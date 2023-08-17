using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ComponentRemoveCommand : ComponentCommand {
        public ComponentRemoveCommand() { }

        public ComponentRemoveCommand(Entity entity, Type componentType)
            : base(entity, componentType) { }

        public override void Execute(Engine engine) => Entity.RemoveComponentWithoutEvent(ComponentType);

        public override string ToString() =>
            string.Format("ComponentRemoveCommand Entity={0} ComponentType={1}", Entity, ComponentType);
    }
}