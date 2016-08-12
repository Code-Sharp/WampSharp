#if CASTLE || DISPATCH_PROXY
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Castle.DynamicProxy;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;
using WampSharp.V2.Core;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class CalleeProxyInterceptorBase : IInterceptor
    {
        private readonly MethodInfo mMethod;
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly ICalleeProxyInterceptor mInterceptor;

        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            mMethod = method;
            mHandler = handler;
            mInterceptor = interceptor;
        }

        public ICalleeProxyInterceptor Interceptor
        {
            get
            {
                return mInterceptor;
            }
        }

        public IWampCalleeProxyInvocationHandler Handler
        {
            get
            {
                return mHandler;
            }
        }

        public MethodInfo Method
        {
            get
            {
                return mMethod;
            }
        }

        public abstract object Invoke(MethodInfo method, object[] arguments);

#if CASTLE

        public void Intercept(IInvocation invocation)
        {
            object result = Invoke(invocation.Method, invocation.Arguments);
            invocation.ReturnValue = result;
        }

#endif
    }

    internal abstract class CalleeProxyInterceptorBase<TResult> : CalleeProxyInterceptorBase
    {
        private readonly IOperationResultExtractor<TResult> mExtractor;

        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler,
            ICalleeProxyInterceptor interceptor)
            : base(method, handler, interceptor)
        {
            mExtractor = GetOperationResultExtractor<TResult>(method);
        }

        public IOperationResultExtractor<TResult> Extractor
        {
            get
            {
                return mExtractor;
            }
        }

        private static IOperationResultExtractor<T> GetOperationResultExtractor<T>(MethodInfo method)
        {
            IOperationResultExtractor<T> extractor;

            if (typeof(T).IsValueTuple())
            {
                extractor = GetValueTupleOperationResultExtractor<T>(method);
            }
            else if (!method.HasMultivaluedResult())
            {
                bool hasReturnValue = method.HasReturnValue();
                extractor = new SingleValueExtractor<T>(hasReturnValue);
            }
            else
            {
                Type elementType = typeof(T).GetElementType();

                Type extractorType =
                    typeof(MultiValueExtractor<>).MakeGenericType(elementType);

                extractor =
                    (IOperationResultExtractor<T>)Activator.CreateInstance(extractorType);
            }

            return extractor;
        }

        private static IOperationResultExtractor<T> GetValueTupleOperationResultExtractor<T>(MethodInfo method)
        {
            ArgumentUnpacker unpacker = GetTupleArgumentUnpacker<T>(method);

            return new ValueTupleValueExtractor<T>(unpacker);
        }

        private static ArgumentUnpacker GetTupleArgumentUnpacker<T>(MethodInfo method)
        {
            IEnumerable<LocalParameter> localParameters;
            IEnumerable<string> transformNames;

            bool skipPositionalArguments = false;

            if (!method.ReturnParameter.IsDefined(typeof(TupleElementNamesAttribute)))
            {
                transformNames = Enumerable.Repeat(default(string), typeof(T).GetValueTupleLength());
            }
            else
            {
                TupleElementNamesAttribute attribute =
                    method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                transformNames = attribute.TransformNames;

                skipPositionalArguments = true;
            }

            var argumentTypes =
                typeof(T).GetGenericArguments()
                         .Select((type, index) => new { type, index });

            localParameters =
                transformNames
                    .Zip
                    (argumentTypes,
                     (name, typeToIndex) =>
                         new LocalParameter(name,
                                            typeToIndex.type,
                                            typeToIndex.index));

            ArgumentUnpacker result = new ArgumentUnpacker(localParameters.ToArray());

            result.SkipPositionalArguments = skipPositionalArguments;

            return result;
        }
    }
}
#endif