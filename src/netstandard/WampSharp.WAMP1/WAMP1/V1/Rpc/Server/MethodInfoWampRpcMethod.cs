using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Rpc.Server
{
    /// <summary>
    /// An implementation of <see cref="IWampRpcMethod"/> using <see cref="MethodInfo"/>.
    /// </summary>
    public class MethodInfoWampRpcMethod : IWampRpcMethod
    {
        private readonly object mInstance;
        private readonly Func<object, object[], object> mMethodInvoke;

        /// <summary>
        /// Creates a new instance of <see cref="MethodInfoWampRpcMethod"/>.
        /// </summary>
        /// <param name="instance">The instance that this method will use.</param>
        /// <param name="method">The <see cref="MethodInfo"/> this method wraps.</param>
        /// <param name="baseUri">The base uri of the method.</param>
        public MethodInfoWampRpcMethod(object instance, MethodInfo method, string baseUri)
        {
            mInstance = instance;
            MethodInfo = method;
            mMethodInvoke = MethodInvokeGenerator.CreateInvokeMethod(method);

            ProcUri = GetProcUri(method, baseUri);
        }

        private string GetProcUri(MethodInfo method, string baseUri)
        {
            WampRpcMethodAttribute wampRpcMethodAttribute =
                method.GetCustomAttribute<WampRpcMethodAttribute>(true);

            string attributeUri = 
                wampRpcMethodAttribute.ProcUri;

            if (baseUri == null || !wampRpcMethodAttribute.IsRelative)
            {
                return attributeUri;
            }
            else
            {
                string result = baseUri + attributeUri;
                return result;
            }
        }

        public string Name => MethodInfo.Name;

        public string ProcUri { get; }

        /// <summary>
        /// Gets the <see cref="System.Reflection.MethodInfo"/> this rpc method
        /// is bound to.
        /// </summary>
        public MethodInfo MethodInfo { get; }

        public Type[] Parameters
        {
            get { return MethodInfo.GetParameters().Select(paramterInfo => paramterInfo.ParameterType).ToArray(); }
        }

        /// <summary>
        /// Gets the instance used for <see cref="MethodBase.Invoke(object,object[])"/>
        /// call.
        /// </summary>
        /// <param name="client">The <see cref="IWampClient"/> requesting this call.</param>
        /// <returns>The instance to use for invocation.</returns>
        protected virtual object GetInstance(IWampClient client)
        {
            return mInstance;
        }

        public Task<object> InvokeAsync(IWampClient client, object[] parameters)
        {
            Task<object> result = null;

            if (!typeof (Task).IsAssignableFrom(MethodInfo.ReturnType))
            {
                result = Task.Factory.StartNew(() => Invoke(client, parameters));
            }
            else
            {
                Task task = (Task)Invoke(client, parameters);

                result = task.CastTask();
            }

            return result;
        }

        public object Invoke(IWampClient client, object[] parameters)
        {
            object result;

            result = mMethodInvoke(GetInstance(client), parameters);

            return result;
        }
    }
}