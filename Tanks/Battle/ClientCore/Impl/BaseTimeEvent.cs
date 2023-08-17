using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BaseTimeEvent : Event {
        public double ClientTime { get; set; } = PreciseTime.Time;
    }
}