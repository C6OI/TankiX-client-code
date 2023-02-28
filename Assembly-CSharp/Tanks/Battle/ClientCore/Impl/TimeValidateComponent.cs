using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TimeValidateComponent : Component {
        public TimeValidateComponent() => Time = (int)(PreciseTime.Time * 1000.0);

        public int Time { get; set; }
    }
}