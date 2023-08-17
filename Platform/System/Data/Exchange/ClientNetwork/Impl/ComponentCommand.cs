using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public abstract class ComponentCommand : EntityCommand {
        public ComponentCommand() { }

        public ComponentCommand(Entity entity, Type componentType)
            : base(entity) => ComponentType = componentType;

        [ProtocolParameterOrder(1)] [ProtocolVaried]
        public Type ComponentType { get; set; }

        protected bool Equals(ComponentCommand other) => Equals(ComponentType, other.ComponentType);

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

            return Equals((ComponentCommand)obj);
        }

        public override int GetHashCode() => ComponentType != null ? ComponentType.GetHashCode() : 0;
    }
}