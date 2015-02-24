using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.PubSub
{
    public class MethodInfoSubscriber : LocalSubscriber
    {
        private readonly object mInstance;
        private readonly MethodInfo mMethod;
        private readonly LocalParameter[] mParameters;

        public MethodInfoSubscriber(object instance, MethodInfo method, string topic)
            : base(topic)
        {
            mInstance = instance;
            mMethod = method;

            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (method.ReturnType != typeof (void))
            {
                throw new ArgumentException("Expected return type of void from method " + method.Name,
                    "method");
            }

            if (method.GetParameters().Any(x => x.ParameterType.IsByRef || x.IsOut))
            {
                throw new ArgumentException("Expected not out/ref parameters from method " + method.Name,
                    "method");                
            }

            mParameters = method.GetParameters()
                .Select(x => new LocalParameter(x))
                .ToArray();
        }

        public override LocalParameter[] Parameters
        {
            get
            {
                return mParameters;
            }
        }

        protected override void InnerEvent<TMessage>
            (IWampFormatter<TMessage> formatter,
                long publicationId,
                EventDetails details,
                TMessage[] arguments,
                IDictionary<string, TMessage> argumentsKeywords)
        {
            WampEventContext.Current = new WampEventContext(publicationId, details);

            try
            {
                object[] methodParameters =
                    UnpackParameters(formatter, arguments, argumentsKeywords);

                mMethod.Invoke(mInstance, methodParameters);
            }
            catch (Exception)
            {
                // TODO: Log that something went wrong
            }
            finally
            {
                WampEventContext.Current = null;
            }
        }
    }
}