using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.Rpc.Server
{
    /// <summary>
    /// An implementation of <see cref="IWampRpcMethod"/> using <see cref="MethodInfo"/>.
    /// </summary>
    public class MethodInfoWampRpcMethod : IWampRpcMethod
    {
        private readonly object mInstance;
        private readonly MethodInfo mMethod;
        private readonly string mProcUri;

        /// <summary>
        /// Creates a new instance of <see cref="MethodInfoWampRpcMethod"/>.
        /// </summary>
        /// <param name="instance">The instance that this method will use.</param>
        /// <param name="method">The <see cref="MethodInfo"/> this method wraps.</param>
        /// <param name="baseUri">The base uri of the method.</param>
        public MethodInfoWampRpcMethod(object instance, MethodInfo method, string baseUri)
        {
            mInstance = instance;
            mMethod = method;

            mProcUri = GetProcUri(method, baseUri);
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

        public string Name
        {
            get
            {
                return mMethod.Name;
            }
        }

        public string ProcUri
        {
            get
            {
                return mProcUri;
            }
        }
        
        public Type[] Parameters
        {
            get { return mMethod.GetParameters().Select(paramterInfo => paramterInfo.ParameterType).ToArray(); }
        }

        public Task<object> InvokeAsync(object[] parameters)
        {
            Task<object> result = null;

            if (!typeof (Task).IsAssignableFrom(mMethod.ReturnType))
            {
                result = Task.Factory.StartNew(() => Invoke(parameters));
            }
            else
            {
                var task = (Task)Invoke(parameters);
                
                if (task.GetType() == typeof (Task))
                {
                    result = task.ContinueWith(x => (object) null);
                }
                else
                {
                    result = ConvertTask((dynamic) task);
                }
            }

            return result;
        }

        private Task<object> ConvertTask<T>(Task<T> task)
        {
            return task.ContinueWith(t => (object) t.Result);
        }
        
        public object Invoke(object[] parameters)
        {
            return mMethod.Invoke(mInstance, parameters);
        }
    }
}