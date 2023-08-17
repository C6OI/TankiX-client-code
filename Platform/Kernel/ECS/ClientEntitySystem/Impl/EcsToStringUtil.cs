using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EcsToStringUtil {
        public static string ToString(ICollection<Type> components) {
            StringBuilder stringBuilder = new();
            stringBuilder.Append('[');
            bool flag = true;

            foreach (Type component in components) {
                if (flag) {
                    flag = false;
                } else {
                    stringBuilder.Append(',');
                }

                stringBuilder.Append(component.Name);
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        public static string GetComponentDetails(Component component) {
            PropertyInfo[] properties = component.GetType().GetProperties();
            string name = component.GetType().Name;

            if (properties.Length == 0) {
                return name;
            }

            name += " [";
            PropertyInfo propertyInfo = null;

            try {
                PropertyInfo[] array = properties;

                foreach (PropertyInfo propertyInfo2 in array) {
                    if (IsNotObsolete(propertyInfo2)) {
                        propertyInfo = propertyInfo2;
                        string text = string.Concat(propertyInfo2.Name, ":", propertyInfo2.GetValue(component, null), ", ");
                        name += text;
                    }
                }
            } catch (Exception ex) {
                string text2 = name;
                name = text2 + ex.Message + " property=" + propertyInfo;
            }

            return name.Remove(name.Length - 2) + "]";
        }

        static bool IsNotObsolete(PropertyInfo property) =>
            property.GetCustomAttributes(typeof(ObsoleteAttribute), false).Count().Equals(0);

        public static string ToString(Entity entity) => string.Format("[Name={0},\tid={1}]", entity.Name, entity.Id);

        public static string ToStringWithComponents(EntityInternal entity) => string.Format("[{0},\t{1},\t{2}]",
            entity.Name,
            entity.Id,
            ToString(entity.ComponentClasses));

        public static string ToString(Handler handler) {
            StringBuilder stringBuilder = new();
            MethodInfo method = handler.Method;
            string value = AttributesToString(method.GetCustomAttributes(true));
            stringBuilder.Append(value);
            stringBuilder.Append(" ");
            stringBuilder.Append(method.DeclaringType.Name + "." + method.Name);
            stringBuilder.Append("(" + ToString(method) + ")");
            stringBuilder.Append(" ");
            stringBuilder.Append("\n");
            return stringBuilder.ToString();
        }

        public static string ToString(MethodInfo method) {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(method.DeclaringType.Name).Append("::").Append(method.Name)
                .Append("(");

            ParameterInfo[] parameters = method.GetParameters();

            for (int i = 0; i < parameters.Length; i++) {
                ParameterInfo parameterInfo = parameters[i];

                if (i > 0) {
                    stringBuilder.Append(", ");
                }

                object[] customAttributes = parameterInfo.GetCustomAttributes(true);

                if (customAttributes.Length > 0) {
                    stringBuilder.Append(AttributesToString(customAttributes));
                    stringBuilder.Append(" ");
                }

                if (parameterInfo.ParameterType.IsSubclassOf(typeof(ICollection))) {
                    stringBuilder.Append("Collection<" + parameterInfo.ParameterType.GetGenericArguments()[0].Name + ">");
                } else {
                    stringBuilder.Append(parameterInfo.ParameterType.Name);
                }
            }

            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        public static string AttributesToString(object[] annotations) {
            StringBuilder stringBuilder = new();
            stringBuilder.Append('[');
            bool flag = true;

            foreach (object obj in annotations) {
                if (flag) {
                    flag = false;
                } else {
                    stringBuilder.Append(',');
                }

                stringBuilder.Append(obj.GetType().Name);
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        public static string ToString(ICollection<Handler> handlers) {
            StringBuilder stringBuilder = new();
            stringBuilder.Append('[');
            bool flag = true;

            foreach (Handler handler in handlers) {
                if (flag) {
                    flag = false;
                } else {
                    stringBuilder.Append(',');
                }

                stringBuilder.Append(handler.Method.Name);
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        public static object ToString(object[] objects) {
            StringBuilder stringBuilder = new();
            stringBuilder.Append('[');
            bool flag = true;

            foreach (object value in objects) {
                if (flag) {
                    flag = false;
                } else {
                    stringBuilder.Append(',');
                }

                stringBuilder.Append(value);
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        public static string ToString(Entity[] entities) {
            StringBuilder stringBuilder = new();
            stringBuilder.Append('[');
            bool flag = true;

            foreach (Entity entity in entities) {
                if (flag) {
                    flag = false;
                } else {
                    stringBuilder.Append(',');
                }

                stringBuilder.Append(entity.Id);
                stringBuilder.Append(" ");
                stringBuilder.Append(entity.Name);
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        public static string ToString(ICollection<Entity> entities) => ToString(entities.ToArray());
    }
}