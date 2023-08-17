using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankCollisionDetector : MonoBehaviour {
        public TankCollisionDetectionComponent tankCollisionComponent;

        public int UpdatesCout { get; set; }

        public bool CanBeActivated { get; set; }

        void FixedUpdate() {
            if (UpdatesCout == 0) {
                CanBeActivated = true;
            }

            UpdatesCout++;
        }

        void OnEnable() => UpdatesCout = 0;

        void OnTriggerEnter(Collider other) => CheckCollisionsWithOtherTanks(other);

        void OnTriggerStay(Collider other) => CheckCollisionsWithOtherTanks(other);

        void CheckCollisionsWithOtherTanks(Collider other) {
            if (other.gameObject.layer == Layers.REMOTE_TANK_BOUNDS) {
                CanBeActivated = false;
            }
        }
    }
}