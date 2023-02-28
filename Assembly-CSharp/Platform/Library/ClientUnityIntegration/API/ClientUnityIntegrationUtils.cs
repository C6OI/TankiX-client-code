using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Platform.Library.ClientUnityIntegration.API {
    public static class ClientUnityIntegrationUtils {
        [Inject] public static EngineServiceInternal EngineServiceInternal { get; set; }

        public static bool HasEngine() => EngineServiceInternal != null;

        public static bool HasWorkingEngine() => EngineServiceInternal != null && EngineServiceInternal.IsRunning;

        public static void CollectComponents(this GameObject gameObject, Entity entity) {
            Component[] components = gameObject.GetComponents(typeof(Kernel.ECS.ClientEntitySystem.API.Component));
            Component[] array = components;

            foreach (Component component in array) {
                ((EntityInternal)entity).AddComponent((Kernel.ECS.ClientEntitySystem.API.Component)component);
            }
        }

        public static void CollectComponentsInChildren(this GameObject gameObject, Entity entity) {
            Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Kernel.ECS.ClientEntitySystem.API.Component));
            Component[] array = componentsInChildren;

            foreach (Component component in array) {
                ((EntityInternal)entity).AddComponent((Kernel.ECS.ClientEntitySystem.API.Component)component);
            }
        }

        public static void ExecuteInFlow(Consumer<Engine> action) {
            if (!HasEngine()) {
                Debug.LogError("Engine does not exist");
            } else {
                Flow.Current.ScheduleWith(action);
            }
        }
    }
}