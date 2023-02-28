using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    public class TankJumpingSystem : ECSSystem {
        [OnEventFire]
        public void ControlJump(FixedUpdateEvent e, JumpingContolledTankNode tank) {
            if (tank.tankJump.isFinished()) {
                tank.Entity.RemoveComponent<TankJumpComponent>();
            } else if (tank.tankJump.isBegin()) {
                tank.rigidbody.Rigidbody.velocity = tank.tankJump.Velocity;
            }
        }

        public class JumpingContolledTankNode : Node {
            public RigidbodyComponent rigidbody;

            public TankJumpComponent tankJump;
            public TankSyncComponent tankSync;
        }
    }
}