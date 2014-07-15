using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.Rpc.Client
{
    internal static class TaskExtensions
    {
        private static readonly MethodInfo mCastTask = GetCastTaskMethod();

        private static MethodInfo GetCastTaskMethod()
        {
            return typeof(TaskExtensions).GetMethod("InternalCastTask",
                                                     BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static Task Cast(this Task<object> task, Type taskType)
        {
            return (Task)mCastTask.MakeGenericMethod(taskType).Invoke(null, new object[] { task });
        }

        private static Task<T> InternalCastTask<T>(Task<object> task)
        {
            return task.ContinueWithSafe(x => (T)x.Result);
        }

        /// <summary>
        /// Casts a <see cref="Task"/> to a Task of type Task{object}.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Task<object> CastTask(this Task task)
        {
            Task<object> result;

            if (task.GetType() == typeof(Task))
            {
                result = task.ContinueWithSafe(x => (object)null);
            }
            else
            {
                result = InnerCastTask((dynamic)task);
            }

            return result;
        }

        private static Task<object> InnerCastTask<T>(Task<T> task)
        {
            return task.ContinueWithSafe(t => (object)t.Result);
        }

        private static Task<TResult> ContinueWithSafe<TTask, TResult>(this TTask task, Func<TTask, TResult> transform)
            where TTask : Task
        {
            return task.ContinueWith(t => ContinueWithSafeCallback((TTask)t, transform),
                                     TaskContinuationOptions.ExecuteSynchronously);
        }

        private static TResult ContinueWithSafeCallback<TTask, TResult>(TTask task, Func<TTask, TResult> transform)
            where TTask : Task
        {
            AggregateException aggregateException = task.Exception;

            if (aggregateException != null)
            {
                throw aggregateException.InnerException;
            }

            TResult result = transform(task);

            return result;
        }
    }
}