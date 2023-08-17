using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    [SerialVersionUID(-615965945505672897L)]
    [Shared]
    public class TankMovementComponent : Component {
        public Movement Movement { get; set; }

        public MoveControl MoveControl { get; set; }

        public float WeaponRotation { get; set; }

        public float WeaponControl { get; set; }
    }
}