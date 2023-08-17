using System;
using System.Linq.Expressions;
using System.Reflection;

namespace YamlDotNet.Serialization.Utilities {
    public sealed class GenericInstanceMethod<TInstance> {
        readonly MethodInfo methodToCall;

        public GenericInstanceMethod(Expression<Action<TInstance>> methodCall) {
            MethodCallExpression methodCallExpression = (MethodCallExpression)methodCall.Body;
            methodToCall = methodCallExpression.Method.GetGenericMethodDefinition();
        }

        public object Invoke(Type[] genericArguments, TInstance instance, params object[] arguments) {
            try {
                return methodToCall.MakeGenericMethod(genericArguments).Invoke(instance, arguments);
            } catch (TargetInvocationException ex) {
                throw ex.Unwrap();
            }
        }
    }
}