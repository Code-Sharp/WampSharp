using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WampSharp.Core.Utilities;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.DelegatePubSub
{
    internal class EventHandlerGenerator
    {
        private static readonly MethodInfo mPublishPositionalMethod =
            Method.Get((IWampTopicProxy topicProxy) =>
                topicProxy.Publish(default(PublishOptions), default(object[])));

        private static readonly MethodInfo mPublishKeywordsMethod =
            Method.Get((IWampTopicProxy topicProxy) =>
                topicProxy.Publish(default(PublishOptions), default(object[]), default(IDictionary<string, object>)));

        private static readonly MethodInfo mAddMethod =
            Method.Get((IDictionary<string, object> dictionary) =>
                dictionary.Add(default(string), default(object)));

        private static readonly MethodInfo mCreateKeywordsDelegateMethod =
            Method.Get((EventHandlerGenerator handlerGenerator) =>
                handlerGenerator.CreateKeywordsDelegate<object>(default(IWampTopicProxy), default(PublishOptions)))
                .GetGenericMethodDefinition();

        private static readonly MethodInfo mCreatePositionalDelegateMethod =
            Method.Get((EventHandlerGenerator handlerGenerator) =>
                handlerGenerator.CreatePositionalDelegate<object>(default(IWampTopicProxy), default(PublishOptions)))
                .GetGenericMethodDefinition();

        public Delegate CreatePositionalDelegate(Type delegateType, IWampTopicProxy proxy, PublishOptions publishOptions)
        {
            return (Delegate)mCreatePositionalDelegateMethod.MakeGenericMethod(delegateType)
                .Invoke(this, new object[] { proxy, publishOptions });
        }

        public Delegate CreateKeywordsDelegate(Type delegateType, IWampTopicProxy proxy, PublishOptions publishOptions)
        {
            return (Delegate)mCreateKeywordsDelegateMethod.MakeGenericMethod(delegateType)
                .Invoke(this, new object[] { proxy, publishOptions });
        }

        public TDelegate CreatePositionalDelegate<TDelegate>(IWampTopicProxy proxy, PublishOptions publishOptions)
        {
            // Writes: topicProxy.Publish(publishOptions,
            //                            new object[]
            //                                {
            //                                      (object)arg1,
            //                                      (object)arg2,
            //                                      ...,
            //                                      (object)argn
            //                                });            
            
            Func<ParameterExpression[], Expression> callExpression = parameters =>
                Expression.Call(Expression.Constant(proxy), mPublishPositionalMethod,
                    Expression.Constant(publishOptions),
                    Expression.NewArrayInit(typeof (object),
                        parameters.Select(x => Expression.Convert(x, typeof (object)))));

            return InnerCreateDelegate<TDelegate>(callExpression);
        }

        public TDelegate CreateKeywordsDelegate<TDelegate>(IWampTopicProxy proxy, PublishOptions publishOptions)
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

            Func<ParameterExpression[], Expression> callExpression = parameters =>
                Expression.Call(Expression.Constant(proxy), mPublishKeywordsMethod,
                    Expression.Constant(publishOptions),
                    Expression.NewArrayInit(typeof (object), Enumerable.Empty<Expression>()),
                    Expression.ListInit(Expression.New(typeof (Dictionary<string, object>)),
                        parameters.Select(x => Expression.ElementInit(mAddMethod,
                            Expression.Constant(x.Name, typeof (string)),
                            Expression.Convert(x, typeof (object))))
                        ));

            return InnerCreateDelegate<TDelegate>(callExpression);
        }

        private static TDelegate InnerCreateDelegate<TDelegate>(Func<ParameterExpression[], Expression> callPublishFactory)
        {
            MethodInfo method =
                typeof (TDelegate).GetMethod("Invoke");

            ValidateMethod(method);

            ParameterExpression[] parameters =
                method.GetParameters()
                    .Select(x => Expression.Parameter(x.ParameterType, x.Name))
                    .ToArray();

            Expression callPublish = callPublishFactory(parameters);

            Expression<TDelegate> lambda =
                Expression.Lambda<TDelegate>(callPublish, parameters);

            TDelegate result = lambda.Compile();

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
    }
}