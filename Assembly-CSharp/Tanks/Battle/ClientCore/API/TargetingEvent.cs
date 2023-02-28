using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class TargetingEvent : Event {
        public TargetingEvent() { }

        public TargetingEvent(TargetingData targetingData) => TargetingData = targetingData;

        public TargetingData TargetingData { get; set; }

        public TargetingEvent Init(TargetingData targetingData) {
            TargetingData = targetingData;
            return this;
        }
    }
}