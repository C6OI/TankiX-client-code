using System;
using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ClassUtils {
        public static IList<Type> GetClasses(Type cls, Type to, IList<Type> classes) {
            if (cls == to) {
                return classes;
            }

            classes.Add(cls);
            Type baseType = cls.BaseType;

            while (baseType != to) {
                classes.Add(baseType);
                baseType = baseType.BaseType;

                if (baseType == null) {
                    throw new ArgumentException(string.Concat("cls = ", cls, ", to = ", to));
                }
            }

            return classes;
        }
    }
}