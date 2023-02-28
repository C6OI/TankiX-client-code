using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-1745565482362521070L)]
    public class SpeedComponent : Component {
        public float Speed { get; set; }

        public float TurnSpeed { get; set; }

        public float Acceleration { get; set; }
    }
}