using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IdleKickConfigComponent : Component {
        public int IdleKickTimeSec { get; set; }

        public int IdleWarningTimeSec { get; set; }

        public int CheckPeriodicTimeSec { get; set; }
    }
}