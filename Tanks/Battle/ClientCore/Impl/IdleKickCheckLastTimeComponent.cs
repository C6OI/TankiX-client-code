using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IdleKickCheckLastTimeComponent : Component {
        public Date CheckLastTime { get; set; }
    }
}