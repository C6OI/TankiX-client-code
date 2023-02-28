using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(3169143415222756957L)]
    public class SplashWeaponComponent : Component {
        public float RadiusOfMinSplashDamage { get; set; }

        public float RadiusOfMaxSplashDamage { get; set; }

        public float MinSplashDamagePercent { get; set; }
    }
}