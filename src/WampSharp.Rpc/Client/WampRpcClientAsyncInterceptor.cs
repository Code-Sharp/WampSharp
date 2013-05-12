using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.Core.Dispatch.Handler;

namespace WampSharp.Rpc
{
    internal class WampRpcClientAsyncInterceptor : WampRpcClientInterceptor
    {
        private readonly MethodInfo mConvertTask;

        public WampRpcClientAsyncInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler) : base(serializer, clientHandler)
        {
            mConvertTask =
                this.GetType().GetMethod("ConvertTask",
                                         BindingFlags.Static | BindingFlags.NonPublic);
        }

        public override void Intercept(IInvocation invocation)
        {
            WampRpcCall<object> call =
                Serializer.Serialize(invocation.Method, invocation.Arguments);

            Task<object> task = ClientHandler.HandleAsync(call);

            Type taskType = 
                invocation.Method.ReturnType.GetClosedGenericTypeImplementation(typeof (Task<>));

            Task convertedTask = task;

            if (taskType != null)
            {
                Type genericType =
                    taskType.GetGenericArguments()[0];

                convertedTask = ConvertTask(task, genericType);
            }

            invocation.ReturnValue = convertedTask;
        }

        private Task ConvertTask(Task<object> task, Type taskType)
        {
            return (Task)mConvertTask.MakeGenericMethod(taskType).Invoke(null, new object[] {task});
        }

        private static Task<T> ConvertTask<T>(Task<object> task)
        {
            return task.ContinueWith(x => (T) x.Result);
        }
    }
}