using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public static class InjectionUtils {
        public static void RegisterInjectionPoints(Type injectAttributeType, ServiceRegistry serviceRegistry) {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly[] array = assemblies;

            foreach (Assembly assembly in array) {
                RegisterInjectionPoints(injectAttributeType, serviceRegistry, GetTypes(assembly));
            }
        }

        public static void RegisterInjectionPoints(Type injectAttributeType, ServiceRegistry serviceRegistry, Type[] types) {
            ProcessInjectionPoints(injectAttributeType, types, serviceRegistry.RegisterConsumer);
        }

        public static void ClearInjectionPoints(Type injectionAttributeType) {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly[] array = assemblies;

            foreach (Assembly assembly in array) {
                ProcessInjectionPoints(injectionAttributeType, GetTypes(assembly), Clear);
            }
        }

        public static void ClearInjectionPoints(List<PropertyInfo> props) {
            props.ForEach(Clear);
        }

        public static Type[] GetTypes(Assembly assembly) {
            try {
                return assembly.GetTypes();
            } catch (Exception) {
                return new Type[0];
            }
        }

        public static List<PropertyInfo> GetInjectableProps(Assembly assembly, Type injectAttributeType) {
            List<PropertyInfo> props = new();
            Type[] types = GetTypes(assembly);

            ProcessInjectionPoints(injectAttributeType, types, delegate(PropertyInfo pi) {
                props.Add(pi);
            });

            return props;
        }

        static void Clear(PropertyInfo propertyInfo) {
            MethodInfo setMethod = propertyInfo.GetSetMethod(true);

            if (setMethod.IsStatic) {
                if (setMethod.ContainsGenericParameters) {
                    Debug.LogError("Fail to inject to generic class " + setMethod.ReflectedType);
                }

                setMethod.Invoke(null, new object[1]);
            }
        }

        static void ProcessInjectionPoints(Type injectAttributeType, Type[] types, Action<PropertyInfo> action) {
            foreach (Type type in types) {
                BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                PropertyInfo[] properties = type.GetProperties(bindingAttr);
                PropertyInfo[] array = properties;

                foreach (PropertyInfo propertyInfo in array) {
                    if (propertyInfo.IsDefined(injectAttributeType, false)) {
                        action(propertyInfo);
                    }
                }
            }
        }
    }
}