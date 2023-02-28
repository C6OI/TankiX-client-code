using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636364996704090103L)]
    public class RageEffectComponent : Component {
        public int DecreaseCooldownPerKillMS { get; set; }
    }
}