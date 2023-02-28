using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class WallhackSystem : ECSSystem {
        [OnEventComplete]
        public void check(AfterFixedUpdateEvent e, TankNode tank) {
            if (tank.tankMovementSender.LastSentMovement.HasValue) {
                Vector3 position = tank.tankMovementSender.LastSentMovement.Value.Position;
                Rigidbody rigidbody = tank.rigidbody.Rigidbody;
                Vector3 worldCenterOfMass = rigidbody.worldCenterOfMass;
                Vector3 vector = worldCenterOfMass - position;

                if (!(vector.sqrMagnitude < 0.0001) && Physics.SphereCast(new Ray(position, vector.normalized), 0.04f, vector.magnitude, LayerMasks.STATIC)) {
                    ScheduleEvent<SendTankMovementEvent>(tank);
                }
            }
        }

        public class TankNode : Node {
            public RigidbodyComponent rigidbody;

            public TankMovementSenderComponent tankMovementSender;
            public TankSyncComponent tankSync;
        }
    }
}