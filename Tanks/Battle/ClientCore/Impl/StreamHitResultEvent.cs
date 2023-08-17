using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class StreamHitResultEvent : Event {
        public StreamHitResultEvent() { }

        public StreamHitResultEvent(TargetingData targetingData) => TargetingData = targetingData;

        public TargetingData TargetingData { get; set; }
    }
}