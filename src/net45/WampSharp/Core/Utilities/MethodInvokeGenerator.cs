using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.Core.Utilities
{
    internal static class MethodInvokeGenerator
    {
        private static readonly MethodInvokeGenerator<object> mInvokeGenerator = new MethodInvokeGenerator<object>();
        private static TaskMethodInvokeGenerator mTaskInvokeGenerator = new TaskMethodInvokeGenerator(); 

        public static Func<object, object[], object> CreateInvokeMethod(MethodInfo method)
        {
            return mInvokeGenerator.CreateInvokeMethod(method);
        }

        public static Func<object, object[], Task> CreateTaskInvokeMethod(MethodInfo method)
        {
            return mTaskInvokeGenerator.CreateInvokeMethod(method);
        }
    }

    internal class MethodInvokeGenerator<TResult>
    {
        public Func<object, object[], TResult> CreateInvokeMethod(MethodInfo method)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object));
            ParameterExpression arguments = Expression.Parameter(typeof(object[]));

            ParameterInfo[] parameters = method.GetParameters();

            List<ParameterExpression> argumentVariables = new List<ParameterExpression>();
            List<Expression> arrayUnpack = new List<Expression>();
            List<Expression> arrayPack = new List<Expression>();

            FillArgumentsExpressions(arguments,
                parameters,
                argumentVariables,
                arrayUnpack,
                arrayPack);

            var castedInstance = Expression.Convert(instance, method.DeclaringType);

            var call = Expression.Call(castedInstance, method, argumentVariables);

            var resultVariable = Expression.Variable(typeof(TResult));
            var returnValueStatements = GetReturnValueStatements(method, call, resultVariable);

            Func<object, object[], TResult> invoker =
                GetInvokerBody(instance,
                    arguments, argumentVariables, arrayUnpack, returnValueStatements, arrayPack, resultVariable);

            return invoker;
        }

        private static Func<object, object[], TResult> GetInvokerBody
            (ParameterExpression instance,
                ParameterExpression arguments,
                IEnumerable<ParameterExpression> argumentVariables,
                IEnumerable<Expression> arrayUnpack,
                IEnumerable<Expression> returnValueStatements,
                IEnumerable<Expression> arrayPack,
                ParameterExpression resultVariable)
        {
            var methodVariables = new List<ParameterExpression>();
            methodVariables.AddRange(argumentVariables);
            methodVariables.Add(resultVariable);

            var bodyExpressions = new List<Expression>();
            bodyExpressions.AddRange(arrayUnpack);
            bodyExpressions.AddRange(returnValueStatements);
            bodyExpressions.AddRange(arrayPack);
            bodyExpressions.Add(resultVariable);

            var body = Expression.Block(typeof(TResult), methodVariables, bodyExpressions);
            var lambda = Expression.Lambda<Func<object, object[], TResult>>(body, instance, arguments);

            Func<object, object[], TResult> invoker = lambda.Compile();
            return invoker;
        }

        protected virtual ICollection<Expression> GetReturnValueStatements(MethodInfo method,
            Expression call,
            Expression resultVariable)
        {
            var returnValueStatements = new List<Expression>();

            if (method.ReturnType == typeof(void))
            {
                returnValueStatements.Add(call);
                returnValueStatements.Add(Expression.Assign(resultVariable, Expression.Constant(null, typeof(object))));
            }
            else if (typeof(TResult).IsValueType() != method.ReturnType.IsValueType())
            {
                returnValueStatements.Add(Expression.Assign(resultVariable, Expression.Convert(call, typeof(TResult))));
            }
            else
            {
                returnValueStatements.Add(Expression.Assign(resultVariable, call));
            }

            return returnValueStatements;
        }

        private void FillArgumentsExpressions
            (Expression arguments,
                IEnumerable<ParameterInfo> parameters,
                ICollection<ParameterExpression> argumentVariables,
                ICollection<Expression> arrayUnpack,
                ICollection<Expression> arrayPack)
        {
            foreach (ParameterInfo parameter in parameters)
            {
                Type parameterType = parameter.ParameterType.StripByRef();

                var variable =
                    Expression.Variable(parameterType,
                        parameter.Name);

                argumentVariables.Add(variable);

                var arrayAccess = Expression.ArrayAccess(arguments,
                    Expression.Constant(parameter.Position));

                if (!parameter.IsOut)
                {
                    var cast = Expression.Convert(arrayAccess, parameterType);
                    var unpack = Expression.Assign(variable, cast);
                    arrayUnpack.Add(unpack);
                }

                if (parameter.IsOut || parameter.ParameterType.IsByRef)
                {
                    Expression boxedVariable = variable;

                    if (parameterType.IsValueType())
                    {
                        boxedVariable =
                            Expression.Convert(boxedVariable, typeof(object));
                    }

                    var pack = Expression.Assign(arrayAccess, boxedVariable);

                    arrayPack.Add(pack);
                }
            }
        }
    }
}