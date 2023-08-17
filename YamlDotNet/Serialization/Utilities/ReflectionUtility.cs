using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace YamlDotNet.Serialization.Utilities {
    static class ReflectionUtility {
        public static Type GetImplementedGenericInterface(Type type, Type genericInterfaceType) {
            foreach (Type implementedInterface in GetImplementedInterfaces(type)) {
                if (implementedInterface.IsGenericType() &&
                    implementedInterface.GetGenericTypeDefinition() == genericInterfaceType) {
                    return implementedInterface;
                }
            }

            return null;
        }

        public static IEnumerable<Type> GetImplementedInterfaces(Type type) {
            if (type.IsInterface()) {
                yield return type;
            }

            Type[] interfaces = type.GetInterfaces();

            for (int i = 0; i < interfaces.Length; i++) {
                yield return interfaces[i];
            }
        }

        public static MethodInfo GetMethod(Expression<Action> methodAccess) {
            MethodInfo methodInfo = ((MethodCallExpression)methodAccess.Body).Method;

            if (methodInfo.IsGenericMethod) {
                methodInfo = methodInfo.GetGenericMethodDefinition();
            }

            return methodInfo;
        }

        public static MethodInfo GetMethod<T>(Expression<Action<T>> methodAccess) {
            MethodInfo methodInfo = ((MethodCallExpression)methodAccess.Body).Method;

            if (methodInfo.IsGenericMethod) {
                methodInfo = methodInfo.GetGenericMethodDefinition();
            }

            return methodInfo;
        }
    }
}