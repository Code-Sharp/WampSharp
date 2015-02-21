using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.DelegatePubSub
{
    public class PublisherDelegateFactory<TDelegate>
    {
        private static readonly DelegateFactory<TDelegate> mPublishArrayFactory =
            EventHandlerGenerator.CreateArrayDelegate<TDelegate>();

        private static readonly DelegateFactory<TDelegate> mPublishArgumentsFactory =
            EventHandlerGenerator.CreateArgumentsDelegate<TDelegate>();

        public static TDelegate CreateArrayHandler(IWampTopicProxy topicProxy, PublishOptions options)
        {
            return mPublishArrayFactory.Invoke(topicProxy, options);
        }

        public static TDelegate CreateArgumentsHandler(IWampTopicProxy topicProxy, PublishOptions options)
        {
            return mPublishArgumentsFactory.Invoke(topicProxy, options);
        }
    }

    internal delegate TDelegate DelegateFactory<TDelegate>(IWampTopicProxy topicProxy, PublishOptions options);


    internal class EventHandlerGenerator
    {
        private static readonly MethodInfo mPublishArrayMethod =
            Method.Get((IWampTopicProxy topicProxy) =>
                topicProxy.Publish(default(PublishOptions), default(object[])));

        private static readonly MethodInfo mPublishArgumentsMethod =
            Method.Get((IWampTopicProxy topicProxy) =>
                topicProxy.Publish(default(PublishOptions), default(object[]), default(IDictionary<string, object>)));

        private static readonly MethodInfo mAddMethod =
            Method.Get((IDictionary<string, object> dictionary) =>
                dictionary.Add(default(string), default(object)));

        public static DelegateFactory<TDelegate> CreateArrayDelegate<TDelegate>()
        {
            // Writes: topicProxy.Publish(publishOptions,
            //                            new object[]
            //                                {
            //                                      (object)arg1,
            //                                      (object)arg2,
            //                                      ...,
            //                                      (object)argn
            //                                });            
            Func<Expression, Expression, ParameterExpression[], Expression> callExpression = 
                (topicProxy, publishOptions, parameters) =>
                Expression.Call(topicProxy, mPublishArrayMethod,
                    publishOptions,
                    Expression.NewArrayInit(typeof (object),
                        parameters.Select(x => Expression.Convert(x, typeof (object)))));

            return InnerCreateDelegate<TDelegate>(callExpression);
        }

        public static DelegateFactory<TDelegate> CreateArgumentsDelegate<TDelegate>()
        {
            // Writes: topicProxy.Publish(publishOptions,
            //                            new object[0],
            //                            new Dictionary<string, object>()
            //                                {
            //                                  {"@arg1", arg1}, 
            //                                  {"@arg2", arg2}, 
            //                                  {"@arg3", arg3},
            //                                  ....,
            //                                  {"@argn", argn}
            //                                });
            // where @arg1, @arg2, ..., @argn are the names of the delegate variables.

            Func<Expression, Expression, ParameterExpression[], Expression> callExpression =
                (topicProxy, publishOptions, parameters) =>
                    Expression.Call(topicProxy, mPublishArgumentsMethod,
                        publishOptions,
                        Expression.NewArrayInit(typeof (object), Enumerable.Empty<Expression>()),
                        Expression.ListInit(Expression.New(typeof (Dictionary<string, object>)),
                            parameters.Select(x => Expression.ElementInit(mAddMethod,
                                Expression.Constant(x.Name, typeof (string)),
                                Expression.Convert(x, typeof (object))))
                            ));

            return InnerCreateDelegate<TDelegate>(callExpression);
        }

        private static DelegateFactory<TDelegate> InnerCreateDelegate<TDelegate>(
            Func<Expression, Expression, ParameterExpression[], Expression> callPublishFactory)
        {
            MethodInfo method =
                typeof (TDelegate).GetMethod("Invoke");

            ValidateMethod(method);

            ParameterExpression topicProxy =
                Expression.Parameter(typeof (IWampTopicProxy), "topicProxy");

            ParameterExpression publishOptions =
                Expression.Parameter(typeof(PublishOptions), "publishOptions");

            ParameterExpression[] parameters =
                method.GetParameters()
                    .Select(x => Expression.Parameter(x.ParameterType, x.Name))
                    .ToArray();

            Expression callPublish = callPublishFactory(topicProxy, publishOptions, parameters);

            Expression<TDelegate> lambda =
                Expression.Lambda<TDelegate>(callPublish, parameters);

            Expression<DelegateFactory<TDelegate>> delegateCreator =
                Expression.Lambda<DelegateFactory<TDelegate>>(lambda, topicProxy, publishOptions);

            DelegateFactory<TDelegate> result = delegateCreator.Compile();

            return result;
        }

        private static void ValidateMethod(MethodInfo method)
        {
            Type delegateType = method.DeclaringType;

            if (method.ReturnType != typeof(void))
            {
                throw new ArgumentException("Expected return type of void from delegate type " + delegateType.FullName);
            }

            if (method.GetParameters().Any(x => x.IsOut || x.ParameterType.IsByRef))
            {
                throw new ArgumentException("Expected no out/ref parameters from delegate type " + delegateType.FullName);                
            }
        }

        private static class Method
        {
            public static MethodInfo Get<T>(Expression<Action<T>> methodCall)
            {
                MethodCallExpression callExpression = methodCall.Body as MethodCallExpression;

                return callExpression.Method;
            }
        }
    }
}