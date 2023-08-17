using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AutoRemoveComponentsRegistryImpl : EngineHandlerRegistrationListener, AutoRemoveComponentsRegistry {
        readonly HashSet<Type> componentTypes = new();

        public AutoRemoveComponentsRegistryImpl(EngineService engineService) =>
            engineService.AddSystemProcessingListener(this);

        public bool IsComponentAutoRemoved(Type componentType) => componentTypes.Contains(componentType);

        public void OnHandlerAdded(Handler handler) {
            if (handler.EventType != typeof(NodeRemoveEvent)) {
                return;
            }

            foreach (HandlerArgument contextArgument in handler.ContextArguments) {
                ICollection<Type> components = contextArgument.NodeDescription.Components;

                if (!IsNodeAutoRemoved(components)) {
                    RegisterOneComponent(components);
                }
            }
        }

        public bool IsNodeAutoRemoved(ICollection<Type> components) => components.Any(componentTypes.Contains);

        void RegisterOneComponent(ICollection<Type> components) {
            foreach (Type component in components) {
                if (!component.IsDefined(typeof(SkipAutoRemove), true)) {
                    componentTypes.Add(component);
                    break;
                }
            }
        }
    }
}