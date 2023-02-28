using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AmbientInnerSoundZone : AmbientSoundZone {
        [SerializeField] Collider[] zoneColliders;

        int collidersLength;

        public void InitInnerZone() {
            collidersLength = zoneColliders.Length;

            for (int i = 0; i < collidersLength; i++) {
                Collider collider = zoneColliders[i];
                collider.transform.SetParent(null, true);
            }
        }

        public void FinalizeInnerZone() {
            for (int i = 0; i < collidersLength; i++) {
                Collider collider = zoneColliders[i];
                DestroyObject(collider.gameObject);
            }
        }

        public bool IsActualZone(Transform listener) {
            for (int i = 0; i < collidersLength; i++) {
                Collider collider = zoneColliders[i];

                if (collider.bounds.Contains(listener.position)) {
                    return true;
                }
            }

            return false;
        }
    }
}