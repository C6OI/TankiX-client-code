using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class CooldownTimerComponent : Component {
        public float CooldownTimerSec { get; set; }
    }
}