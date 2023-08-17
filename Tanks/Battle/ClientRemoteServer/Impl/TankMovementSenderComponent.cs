using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class TankMovementSenderComponent : Component {
        public double LastSentMovementTime { get; set; }

        public double LastSentWeaponRotationTime { get; set; }

        public Movement? LastSentMovement { get; set; }

        public bool LastHasCollision { get; set; }

        public Movement LastPhysicsMovement { get; set; }
    }
}