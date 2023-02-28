using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AutoRemoveComponentsSystem : ECSSystem {
        static AutoRemoveComponentsRegistry registry;

        public AutoRemoveComponentsSystem(AutoRemoveComponentsRegistry autoRemoveComponentsRegistry) => registry = autoRemoveComponentsRegistry;

        [OnEventComplete]
        public void AutoRemoveComponentsIfNeed(NodeAddedEvent e, SingleNode<DeletedEntityComponent> node) {
            List<Type> componentsToRemove = GetComponentsToRemove(node);

            if (componentsToRemove.Count > 0) {
                ScheduleEvent(new AutoRemoveComponentsEvent(componentsToRemove), node);
            }
        }

        [OnEventFire]
        public void RemoveComponents(AutoRemoveComponentsEvent e, Node node) {
            List<Type> componentsToRemove = e.ComponentsToRemove;
            componentsToRemove.Sort(new ComponentRemoveOrderComparer());

            foreach (Type item in componentsToRemove) {
                if (node.Entity.HasComponent(item)) {
                    node.Entity.RemoveComponent(item);
                }
            }
        }

        [OnEventComplete]
        public void RepeatRemoveComponents(AutoRemoveComponentsEvent e, Node node) {
            List<Type> componentsToRemove = GetComponentsToRemove(node);

            if (componentsToRemove.Count > 0) {
                ScheduleEvent(new AutoRemoveComponentsEvent(componentsToRemove), node);
            }
        }

        static List<Type> GetComponentsToRemove(Node node) {
            ICollection<Type> componentClasses = ((EntityInternal)node.Entity).ComponentClasses;
            return componentClasses.Where(componentType => registry.IsComponentAutoRemoved(componentType) && componentType != typeof(DeletedEntityComponent)).ToList();
        }
    }
}