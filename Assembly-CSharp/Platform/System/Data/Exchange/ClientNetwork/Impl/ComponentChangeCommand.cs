using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ComponentChangeCommand : EntityCommand {
        public ComponentChangeCommand() { }

        public ComponentChangeCommand(Entity entity, Component component)
            : base(entity) => Component = component;

        [ProtocolVaried] [ProtocolParameterOrder(1)]
        public Component Component { get; set; }

        public ComponentChangeCommand Init(Entity entity, Component component) {
            Component = component;
            Entity = (EntityInternal)entity;
            return this;
        }

        public override void Execute(Engine engine) {
            ApplyChange(engine);
        }

        void ApplyChange(Engine engine) {
            ((EntityImpl)Entity).ChangeComponent(Component);
        }

        public override string ToString() => string.Format("ComponentChangeCommand Entity={0} Component={1}", Entity, EcsToStringUtil.ToStringWithProperties(Component));
    }
}