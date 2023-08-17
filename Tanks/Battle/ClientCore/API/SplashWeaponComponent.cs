using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(3169143415222756957L)]
    [Shared]
    public class SplashWeaponComponent : Component {
        public float RadiusOfMinSplashDamage { get; set; }

        public float RadiusOfMaxSplashDamage { get; set; }

        public float MinSplashDamagePercent { get; set; }
    }
}