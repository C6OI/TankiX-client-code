using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1432792458422L)]
    public class WeaponRotationComponent : Component {
        public float Speed { get; set; }

        public float Acceleration { get; set; }

        public float BaseSpeed { get; set; }
    }
}