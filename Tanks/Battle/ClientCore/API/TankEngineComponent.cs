using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635824351676425226L)]
    public class TankEngineComponent : Component {
        public float MovingBorder { get; set; }

        public float Value { get; set; }

        public float CollisionTimerSec { get; set; }

        public bool HasValuableCollision { get; set; }
    }
}