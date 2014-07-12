﻿using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.Core.Utilities
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
            return task.ContinueWith(x => CastResult<T>(x),
                                     TaskContinuationOptions.ExecuteSynchronously);
        }

        private static T CastResult<T>(Task<object> x)
        {
            if (x.Exception != null)
            {
                throw x.Exception.InnerException;
            }

            return (T)x.Result;
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
                result = task.ContinueWith(x => (object) null,
                                           TaskContinuationOptions.ExecuteSynchronously);
            }
            else
            {
                result = CastTask((dynamic) task);
            }

            return result;
        }

        private static Task<object> CastTask<T>(Task<T> task)
        {
            return task.ContinueWith(t => (object) t.Result,
                                     TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}