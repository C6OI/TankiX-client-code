using System;

namespace Platform.Library.ClientProtocol.API {
    public static class ReflectionUtils {
        public static bool IsNullableType(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static Type GetNullableInnerType(Type nullableType) => nullableType.GetGenericArguments()[0];
    }
}