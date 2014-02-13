using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.V2.Rpc
{
    public class MethodInfoRpcOperation<TMessage> : 
        IWampRpcOperation<TMessage> where TMessage : class
    {
        private readonly MethodInfo mMethod;
        private readonly object mInstance;
        private readonly bool mIsSyncMethod;

        public MethodInfoRpcOperation(MethodInfo method,
                                      object instance)
        {
            mMethod = method;
            
            this.Procedure =
                method.GetCustomAttribute<WampProcedureAttribute>(true).Procedure;

            mInstance = instance;
            mIsSyncMethod = !typeof (Task).IsAssignableFrom(mMethod.ReturnType);
        }

        public string Procedure
        {
            get; 
            private set;
        }

        public void Invoke(IWampRpcOperationCallback caller, TMessage options)
        {
            Invoke(caller, options, null);
        }

        public async void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments)
        {
            if (!IsSyncMethod)
            {
                HandleAsync(caller, arguments);
            }
            else
            {
                // Handle sync?
            }
        }

        private async Task HandleAsync(IWampRpcOperationCallback caller, TMessage[] arguments)
        {
            // TODO: Convert parameters.
            TMessage[] parameters = arguments;

            Task<object> task = InvokeAsync(parameters);

            try
            {
                object result = await task;
                caller.Result(result);
            }
            catch (Exception ex)
            {
                // TODO: unwrap details from exception
                Exception details = ex;
                string error = ex.Message;
                caller.Error(details, error);
            }
        }

        public async void Invoke(IWampRpcOperationCallback caller,
                                 TMessage options,
                                 TMessage[] arguments,
                                 TMessage argumentsKeywords)
        {
            // TODO: order the arguments by the keywords.
            TMessage[] orderedArguments = arguments;

            Invoke(caller, options, orderedArguments);
        }

        private Task<object> InvokeAsync(object[] parameters)
        {
            Task<object> result = null;

            Task task = (Task) InvokeSync(parameters);

            if (task.GetType() != typeof (Task))
            {
                result = ConvertTask((dynamic) task);
            }
            else
            {
                result =
                    task.ContinueWith(x =>
                                          {
                                              x.Wait();
                                              return (object) null;
                                          });
            }

            return result;
        }

        private bool IsSyncMethod
        {
            get
            {
                return mIsSyncMethod;
            }
        }

        private object InvokeSync(object[] arguments)
        {
            try
            {
                return mMethod.Invoke(mInstance, arguments);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        private Task<object> ConvertTask<T>(Task<T> task)
        {
            return task.ContinueWith(t => (object)t.Result);
        }
    }
}