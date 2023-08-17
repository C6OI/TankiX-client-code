using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ComponentAddCommand : EntityCommand {
        public ComponentAddCommand() { }

        public ComponentAddCommand(Entity entity, Component component)
            : base(entity) => Component = component;

        [ProtocolParameterOrder(1)] [ProtocolVaried]
        public Component Component { get; set; }

        public override void Execute(Engine engine) => Entity.AddComponentWithoutEvent(Component);

        protected bool Equals(ComponentAddCommand other) => Component == other.Component;

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != GetType()) {
                return false;
            }

            return Equals((ComponentAddCommand)obj);
        }

        public override int GetHashCode() => 0;

        public override string ToString() =>
            string.Format("ComponentAddCommand: Entity={0} Component={1}", Entity, Component);
    }
}