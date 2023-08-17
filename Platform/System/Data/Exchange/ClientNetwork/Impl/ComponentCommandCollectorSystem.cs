using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ComponentCommandCollectorSystem : AbstractCommandCollectorSystem {
        readonly ComponentAndEventRegistrator componentAndEventRegistrator;

        readonly SharedEntityRegistry entityRegistry;

        public ComponentCommandCollectorSystem(CommandCollector commandCollector,
            ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry)
            : base(commandCollector) {
            this.componentAndEventRegistrator = componentAndEventRegistrator;
            this.entityRegistry = entityRegistry;
        }

        [OnEventFire]
        public void ComponentAdded(ComponentAddedEvent e, Node node) {
            Component component = e.Component;

            if (Allow(component.GetType(), node.Entity)) {
                AddCommand(new ComponentAddCommand((EntityInternal)node.Entity, component));
            }
        }

        [OnEventComplete]
        public void ComponentRemoved(RemoveComponentEvent e, Node node) {
            if (Allow(e.ComponentType, node.Entity)) {
                AddCommand(new ComponentRemoveCommand(node.Entity, e.ComponentType));
            }
        }

        [OnEventFire]
        public void ComponentChanged(ChangeComponentEvent e, Node node) {
            if (Allow(e.ComponentType, node.Entity)) {
                AddCommand(new ComponentChangeCommand(node.Entity, e.Component));
            }
        }

        bool Allow(Type component, Entity entity) => entityRegistry.IsShared(entity.Id) &&
                                                     componentAndEventRegistrator.IsShared(component) &&
                                                     !entity.HasComponent<DeletedEntityComponent>();
    }
}