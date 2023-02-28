using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class FlyingTankSystem : ECSSystem {
        const float MINIMAL_HEIGHT = 0.02f;

        [OnEventFire]
        public void Fall(UpdateEvent e, FlyingTank tank) {
            Ray ray = default;
            ray.origin = tank.rigidbody.Rigidbody.position;
            ray.direction = Vector3.down;
            Ray ray2 = ray;
            RaycastHit hitInfo;

            if (Physics.Raycast(ray2, out hitInfo, 0.02f, 1 << Layers.STATIC | 1 << Layers.VISUAL_STATIC)) {
                ScheduleEvent<FlyingTankGroundedEvent>(tank);
            }
        }

        public class FlyingTank : Node {
            public FlyingTankComponent flyingTank;

            public RigidbodyComponent rigidbody;
            public TankActiveStateComponent tankActiveState;
        }
    }
}