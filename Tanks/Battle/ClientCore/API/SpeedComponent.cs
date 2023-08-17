using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-1745565482362521070L)]
    public class SpeedComponent : SharedChangeableComponent {
        float acceleration;
        float speed;

        float turnSpeed;

        public float Speed {
            get => speed;
            set {
                speed = value;
                OnChange();
            }
        }

        public float TurnSpeed {
            get => turnSpeed;
            set {
                turnSpeed = value;
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