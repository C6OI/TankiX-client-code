using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TimeValidateComponent : Component {
        public TimeValidateComponent() => Time = PreciseTime.Time;

        public double Time { get; set; }
    }
}