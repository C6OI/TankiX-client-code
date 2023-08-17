using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public abstract class EntityCommand : AbstractCommand {
        public EntityCommand() { }

        public EntityCommand(Entity entity) => Entity = (EntityInternal)entity;

        [ProtocolParameterOrder(0)] public EntityInternal Entity { get; set; }
    }
}