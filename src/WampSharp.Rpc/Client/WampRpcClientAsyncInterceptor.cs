using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

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
                invocation.Method.ReturnType.GetGenericArguments()[0];

            Task convertedTask = ConvertTask(task, taskType);

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