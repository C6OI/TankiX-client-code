using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1438152738643L)]
    public class WeaponBulletShotComponent : Component {
        public float BulletRadius { get; set; }

        public float BulletSpeed { get; set; }
    }
}