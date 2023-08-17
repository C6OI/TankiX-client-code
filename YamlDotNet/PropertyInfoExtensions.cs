using System.Reflection;

namespace YamlDotNet {
    static class PropertyInfoExtensions {
        public static object ReadValue(this PropertyInfo property, object target) => property.GetValue(target, null);
    }
}