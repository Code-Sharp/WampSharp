using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Logging;

using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class MethodInfoSubscriber : LocalSubscriber
    {
        private readonly object mInstance;
        private readonly MethodInfo mMethod;
        private readonly LocalParameter[] mParameters;
        private readonly Func<object, object[], object> mMethodInvoker;
        private readonly ILog mLogger;

        public MethodInfoSubscriber(object instance, MethodInfo method, string topic)
            : base()
        {
            mLogger = LogProvider.GetLogger(typeof(MethodInfoSubscriber) + "." + topic);
            mInstance = instance;
            mMethod = method;

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            mMethodInvoker = MethodInvokeGenerator.CreateInvokeMethod(method);

            if (method.ReturnType != typeof (void))
            {
                throw new ArgumentException("Expected return type of void from method " + method.Name,
                    nameof(method));
            }

            if (method.GetParameters().Any(x => x.ParameterType.IsByRef || x.IsOut))
            {
                throw new ArgumentException("Expected not out/ref parameters from method " + method.Name,
                    nameof(method));                
            }

            mParameters = method.GetParameters()
                .Select(x => new LocalParameter(x))
                .ToArray();
        }

        public override LocalParameter[] Parameters => mParameters;

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

                mMethodInvoker(mInstance, methodParameters);
            }
            catch (Exception ex)
            {
                mLogger.ErrorFormat(ex, "An error occurred while calling {Method}", mMethod);
            }
            finally
            {
                WampEventContext.Current = null;
            }
        }
    }
}