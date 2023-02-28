using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientUnityIntegration.API {
    public static class StateUtils {
        public static void SwitchEntityState<T>(Entity entity, HashSet<Type> states) where T : Component {
            SwitchEntityState(entity, typeof(T), states);
        }

        public static void SwitchEntityState(Entity entity, Type targetState, HashSet<Type> states) {
            RemoveNonStateComponents(entity, targetState, states);

            if (!entity.HasComponent(targetState)) {
                entity.AddComponent(entity.CreateNewComponentInstance(targetState));
            }
        }

        public static void SwitchEntityState(Entity entity, Component component, HashSet<Type> states) {
            Type type = component.GetType();
            RemoveNonStateComponents(entity, type, states);

            if (!entity.HasComponent(type)) {
                entity.AddComponent(component);
            }
        }

        static void RemoveNonStateComponents(Entity entity, Type targetState, HashSet<Type> states) {
            HashSet<Type>.Enumerator enumerator = states.GetEnumerator();

            while (enumerator.MoveNext()) {
                Type current = enumerator.Current;

                if (current != targetState && entity.HasComponent(current)) {
                    entity.RemoveComponent(current);
                }
            }
        }
    }
}