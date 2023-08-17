using System;
using System.Reflection;

namespace Platform.System.Data.Statics.ClientYaml.Impl {
    public static class CloneObjectUtil {
        public static object CloneObject(object objSource) {
            if (objSource == null) {
                return null;
            }

            Type type = objSource.GetType();

            if (type.IsPrimitive || type.IsEnum || type == typeof(string)) {
                return objSource;
            }

            if (type.IsArray) {
                Type type2 = Type.GetType(type.FullName.Replace("[]", string.Empty));
                Array array = (Array)objSource;
                Array array2 = Array.CreateInstance(type2, array.Length);

                for (int i = 0; i < array.Length; i++) {
                    array2.SetValue(CloneObject(array.GetValue(i)), i);
                }

                return array2;
            }

            if (type.IsClass || type.IsValueType) {
                if (!HasDefaultConstructor(type)) {
                    return objSource;
                }

                object obj = Activator.CreateInstance(type);
                CopyFields(type, objSource, obj);

                while (type.BaseType != null) {
                    type = type.BaseType;
                    CopyFields(type, objSource, obj);
                }

                return obj;
            }

            return null;
        }

        public static void CopyFields(Type type, object objSource, object copiedObject) {
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance |
                                                BindingFlags.Public |
                                                BindingFlags.NonPublic |
                                                BindingFlags.FlattenHierarchy);

            FieldInfo[] array = fields;

            foreach (FieldInfo fieldInfo in array) {
                object value = fieldInfo.GetValue(objSource);

                if (value != null) {
                    fieldInfo.SetValue(copiedObject, CloneObject(value));
                }
            }
        }

        public static bool HasDefaultConstructor(Type type) =>
            type.IsValueType ||
            type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null) != null;
    }
}