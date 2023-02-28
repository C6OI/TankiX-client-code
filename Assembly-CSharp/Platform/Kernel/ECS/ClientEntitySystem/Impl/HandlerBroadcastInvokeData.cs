using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerBroadcastInvokeData : HandlerInvokeData {
        public HandlerBroadcastInvokeData(Handler handler, Entity entity)
            : base(handler) => Entity = entity;

        public Entity Entity { get; }
    }
}