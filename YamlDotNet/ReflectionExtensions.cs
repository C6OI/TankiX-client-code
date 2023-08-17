using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YamlDotNet {
    static class ReflectionExtensions {
        static readonly FieldInfo remoteStackTraceField =
            typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

        public static bool IsValueType(this Type type) => type.IsValueType;

        public static bool IsGenericType(this Type type) => type.IsGenericType;

        public static bool IsInterface(this Type type) => type.IsInterface;

        public static bool IsEnum(this Type type) => type.IsEnum;

        public static bool HasDefaultConstructor(this Type type) =>
            type.IsValueType ||
            type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null) != null;

        public static TypeCode GetTypeCode(this Type type) => Type.GetTypeCode(type);

        public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type) {
            BindingFlags instancePublic = BindingFlags.Instance | BindingFlags.Public;
            object result;

            if (type.IsInterface) {
                IEnumerable<PropertyInfo> enumerable = new Type[1] { type }.Concat(type.GetInterfaces())
                    .SelectMany(i => i.GetProperties(instancePublic));

                result = enumerable;
            } else {
                result = type.GetProperties(instancePublic);
            }

            return (IEnumerable<PropertyInfo>)result;
        }

        public static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type) =>
            type.GetMethods(BindingFlags.Static | BindingFlags.Public);

        public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] parameterTypes) =>
            type.GetMethod(name, BindingFlags.Static | BindingFlags.Public, null, parameterTypes, null);

        public static Exception Unwrap(this TargetInvocationException ex) {
            Exception innerException = ex.InnerException;

            if (remoteStackTraceField != null) {
                remoteStackTraceField.SetValue(ex.InnerException, ex.InnerException.StackTrace + "\r\n");
            }

            return innerException;
        }
    }
}