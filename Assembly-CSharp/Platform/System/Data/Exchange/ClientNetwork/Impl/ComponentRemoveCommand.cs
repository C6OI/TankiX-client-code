using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ComponentRemoveCommand : ComponentCommand {
        public ComponentRemoveCommand() { }

        public ComponentRemoveCommand(Entity entity, Type componentType)
            : base(entity, componentType) { }

        public ComponentRemoveCommand Init(Entity entity, Type componentType) {
            ComponentType = componentType;
            Entity = (EntityInternal)entity;
            return this;
        }

        public override void Execute(Engine engine) {
            Entity.RemoveComponentSilent(ComponentType);
        }

        public override string ToString() => string.Format("ComponentRemoveCommand Entity={0} ComponentType={1}", Entity, ComponentType);
    }
}