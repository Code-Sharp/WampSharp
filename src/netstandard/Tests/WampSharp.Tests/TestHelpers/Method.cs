using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WampSharp.Tests.TestHelpers
{
    public static class Method
    {
        public static MethodInfo Get<T>(Expression<Action<T>> expression)
        {
            return Get<Action<T>>(expression);
        }

        public static MethodInfo Get<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return Get<Func<T, TResult>>(expression);
        }

        private static MethodInfo Get<T>(Expression<T> expression)
        {
            Expression body = expression.Body;
            MethodCallExpression callExpression = (MethodCallExpression)body;
            return callExpression.Method;
        }
    }
}