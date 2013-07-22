﻿using System;
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
            return task.ContinueWith(x => (T)x.Result);
        }
    }
}