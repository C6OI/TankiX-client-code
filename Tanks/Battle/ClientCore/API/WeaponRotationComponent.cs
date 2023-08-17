using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1432792458422L)]
    public class WeaponRotationComponent : SharedChangeableComponent {
        float acceleration;
        float speed;

        public float Speed {
            get => speed;
            set {
                speed = value;
                OnChange();
            }
        }

        public float Acceleration {
            get => acceleration;
            set {
                acceleration = value;
                OnChange();
            }
        }
    }
}