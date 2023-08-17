using System;
using System.Linq.Expressions;
using System.Reflection;

namespace YamlDotNet.Serialization.Utilities {
    public sealed class GenericStaticMethod {
        readonly MethodInfo methodToCall;

        public GenericStaticMethod(Expression<Action> methodCall) {
            MethodCallExpression methodCallExpression = (MethodCallExpression)methodCall.Body;
            methodToCall = methodCallExpression.Method.GetGenericMethodDefinition();
        }

        public object Invoke(Type[] genericArguments, params object[] arguments) {
            try {
                return methodToCall.MakeGenericMethod(genericArguments).Invoke(null, arguments);
            } catch (TargetInvocationException ex) {
                throw ex.Unwrap();
            }
        }
    }
}