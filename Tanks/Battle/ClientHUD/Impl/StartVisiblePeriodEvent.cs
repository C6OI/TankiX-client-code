using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class StartVisiblePeriodEvent : Event {
        public StartVisiblePeriodEvent() { }

        public StartVisiblePeriodEvent(float durationInSec) => DurationInSec = durationInSec;

        public float DurationInSec { get; set; }
    }
}