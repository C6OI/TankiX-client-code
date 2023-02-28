using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1542363613520L)]
    public class SplashEffectComponent : Component {
        public bool CanTargetTeammates { get; set; }
    }
}