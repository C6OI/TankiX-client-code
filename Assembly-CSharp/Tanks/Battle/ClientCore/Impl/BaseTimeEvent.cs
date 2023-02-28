using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BaseTimeEvent : Event {
        public int ClientTime { get; set; } = (int)(PreciseTime.Time * 1000.0);
    }
}