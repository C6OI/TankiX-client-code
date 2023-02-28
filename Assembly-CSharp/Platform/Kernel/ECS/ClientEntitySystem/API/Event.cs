using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class Event {
        public override string ToString() => EcsToStringUtil.ToStringWithProperties(this);
    }
}