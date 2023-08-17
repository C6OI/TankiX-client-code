using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(-749662101253908993L)]
    public class DamageEffectComponent : Component {
        public float DamageCoefficient { get; set; }
    }
}