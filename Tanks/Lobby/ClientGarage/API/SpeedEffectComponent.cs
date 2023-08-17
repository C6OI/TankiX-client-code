using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(-8329381807624940326L)]
    [Shared]
    public class SpeedEffectComponent : Component {
        public float Coeff { get; set; }

        public float WeaponRotationSpeedMultiplier { get; set; }
    }
}