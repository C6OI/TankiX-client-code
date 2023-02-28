using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AssemblyTypeCollector {
        public static void CollectEmptyEventTypes(List<Type> eventTypes) {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies) {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types) {
                    if (type.IsSubclassOf(typeof(Event)) && !type.IsAbstract && IsEmptyType(type)) {
                        eventTypes.Add(type);
                    }
                }
            }
        }

        static bool IsEmptyType(Type type) {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            return type.GetFields(bindingAttr).Length == 0 && type.GetProperties(bindingAttr).Length == 0;
        }
    }
}