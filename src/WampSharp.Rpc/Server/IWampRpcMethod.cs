using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.Rpc.Server
{
    public interface IWampRpcMethod
    {
        string Name { get; }

        string ProcUri { get; }

        Task<object> InvokeAsync(object instance, object[] parameters);

        Type[] Parameters { get; }

        object Invoke(object instance, object[] parameters);
    }

    public class MethodInfoWampRpcMethod : IWampRpcMethod
    {
        private readonly MethodInfo mMethod;

        public MethodInfoWampRpcMethod(MethodInfo method)
        {
            mMethod = method;
        }

        public string Name
        {
            get { return mMethod.Name; }
        }

        public string ProcUri
        {
            get
            {
                var wampRpcMethodAttribute = mMethod.GetCustomAttribute<WampRpcMethodAttribute>();
                return wampRpcMethodAttribute.ProcUri;
            }
        }

        public Type[] Parameters
        {
            get { return mMethod.GetParameters().Select(paramterInfo => paramterInfo.ParameterType).ToArray(); }
        }


        public Task<object> InvokeAsync(object instance, object[] parameters)
        {
            Task<object> result = null;

            if (!typeof (Task).IsAssignableFrom(mMethod.ReturnType))
            {
                result = Task.Factory.StartNew(() => Invoke(instance, parameters));
            }
            else
            {
                var task = (Task) Invoke(instance, parameters);
                
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
        
        public object Invoke(object instance, object[] parameters)
        {
            return mMethod.Invoke(instance, parameters);
        }
    }
}