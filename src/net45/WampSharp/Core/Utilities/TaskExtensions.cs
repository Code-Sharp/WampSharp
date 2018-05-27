using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.Core.Utilities
{
    internal static class TaskExtensions
    {
        private static readonly MethodInfo mCastTaskToGenericTask = GetCastTaskToGenericTaskMethod();
        private static readonly MethodInfo mCastToNonGenericTask = GetCastGenericTaskToNonGenericMethod();

        private static MethodInfo GetCastGenericTaskToNonGenericMethod()
        {
            return Method.Get(() => CastGenericTaskToNonGeneric<object>(default(Task<object>)))
                .GetGenericMethodDefinition();
        }

        private static MethodInfo GetCastTaskToGenericTaskMethod()
        {
            return Method.Get(() => CastTaskToGenericTask<object>(default(Task<object>)))
                .GetGenericMethodDefinition();
        }

        public static Task Cast(this Task<object> task, Type taskType)
        {
            return (Task) mCastTaskToGenericTask.MakeGenericMethod(taskType)
                                                .Invoke(null, new object[] {task});
        }

        private static Task<T> CastTaskToGenericTask<T>(Task<object> task)
        {
            return task.ContinueWithSafe(x => (T)x.Result);
        }

        /// <summary>
        /// Unwraps the return type of a given method.
        /// </summary>
        /// <param name="returnType">The given return type.</param>
        /// <returns>The unwrapped return type.</returns>
        /// <example>
        /// void, Task -> object
        /// Task{string} -> string
        /// int -> int
        /// </example>
        public static Type UnwrapReturnType(Type returnType)
        {
            if (returnType == typeof(void) || returnType == typeof(Task))
            {
                return typeof(object);
            }

            Type taskType =
                returnType.GetClosedGenericTypeImplementation(typeof(Task<>));

            if (taskType != null)
            {
                return returnType.GetGenericArguments()[0];
            }

            return returnType;
        }

        public static bool HasReturnValue(this MethodInfo method)
        {
            Type returnType = method.ReturnType;

            if (returnType == typeof(void) || returnType == typeof(Task))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Casts a <see cref="Task"/> to a Task of type Task{object}.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Task<object> CastTask(this Task task)
        {
            Task<object> result;

            if (task.GetType() == typeof (Task))
            {
                result = task.ContinueWithSafe(x => (object) null);
            }
            else
            {
                Type underlyingType = UnwrapReturnType(task.GetType());

                MethodInfo method =
                    mCastToNonGenericTask.MakeGenericMethod(underlyingType);

                result = (Task<object>) method.Invoke(null, new object[] {task});
            }

            return result;
        }

        /// <summary>
        /// Casts a <see cref="Task{TResult}"/> to a Task of type Task{object}.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Task<object> CastTask<TResult>(this Task<TResult> task)
        {
            return task.ContinueWithSafe(x => (object)x.Result);
        }

        /// <summary>
        /// Casts a <see cref="Task{TResult}"/> to a Task of type Task{object}.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Task<object> ContinueWithNull(this Task task)
        {
            return task.ContinueWithSafe(x => (object)null);
        }

        private static Task<object> CastGenericTaskToNonGeneric<T>(Task<T> task)
        {
            return task.ContinueWithSafe(t => (object)t.Result);
        }

        private static Task<TResult> ContinueWithSafe<TTask, TResult>(this TTask task, Func<TTask, TResult> transform)
            where TTask : Task
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (transform == null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            TaskCompletionSource<TResult> taskResult = new TaskCompletionSource<TResult>();

            task.ContinueWith(_ =>
            {
                if (task.IsFaulted)
                {
                    taskResult.TrySetException(task.Exception.InnerExceptions);
                }
                else if (task.IsCanceled)
                {
                    taskResult.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        TResult result = transform(task);
                        taskResult.TrySetResult(result);
                    }
                    catch (Exception ex)
                    {
                        taskResult.TrySetException(ex);
                    }
                }
            },
                TaskContinuationOptions.ExecuteSynchronously);

            return taskResult.Task;
        }
    }
}