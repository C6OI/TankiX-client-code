using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class FlagPhysicsBehaviour : TriggerBehaviour<TankFlagCollisionEvent> {
        void OnTriggerEnter(Collider other) {
            SendEventByCollision(other);
        }

        void OnTriggerExit(Collider other) {
            SendEventByCollision(other);
        }
    }
}