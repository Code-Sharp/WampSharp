using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.Core.Utilities
{
    internal class TaskMethodInvokeGenerator : MethodInvokeGenerator<Task>
    {
        private static readonly MethodInfo mCastTaskToNonGeneric =
            Method.Get(() => TaskExtensions.CastTask<object>(default(Task<object>)))
                .GetGenericMethodDefinition();

        private static readonly MethodInfo mContinueWithNull =
            Method.Get(() => TaskExtensions.ContinueWithNull(default(Task<object>)));

        protected override ICollection<Expression> GetReturnValueStatements(MethodInfo method, Expression call, Expression resultVariable)
        {
            var result = base.GetReturnValueStatements(method, call, resultVariable);
            Expression transformedResult;

            if (!method.HasReturnValue())
            {
                transformedResult =
                    Expression.Call(mContinueWithNull, resultVariable);
            }
            else
            {
                Type taskType = method.ReturnType;
                Type unwrapped = TaskExtensions.UnwrapReturnType(taskType);

                transformedResult =
                    Expression.Call(mCastTaskToNonGeneric.MakeGenericMethod(unwrapped),
                    Expression.Convert(resultVariable, taskType));
            }

            var assign =
                Expression.Assign(resultVariable, transformedResult);

            result.Add(assign);

            return result;
        }
    }
}