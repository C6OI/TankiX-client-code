using System.Linq;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class PhysicsUtil {
        static readonly float HIT_EPS = 0.001f;

        static readonly RaycastHit[] raycastResults = new RaycastHit[1000];

        public static Vector3 GetPulledHitPoint(RaycastHit hitInfo) {
            Vector3 vector = Vector3.Normalize(hitInfo.normal);
            Vector3 point = hitInfo.point;
            return point + HIT_EPS * vector;
        }

        public static bool InsiderRaycast(Vector3 origin, Vector3 dir, out RaycastHit hitInfo, float distance, int mask,
            bool isInsideAttack) {
            Ray ray = new(origin, dir);

            if (!isInsideAttack) {
                return Physics.Raycast(ray, out hitInfo, distance, mask);
            }

            Vector3 point = ray.GetPoint(distance);
            Vector3 direction = -dir;
            int num = Physics.RaycastNonAlloc(point, direction, raycastResults, distance, mask);

            if (num == 0) {
                hitInfo = default;
                return false;
            }

            RaycastHit raycastHit = raycastResults[0];

            for (int i = 0; i < num; i++) {
                RaycastHit raycastHit2 = raycastResults[i];

                if (raycastHit2.distance > raycastHit.distance) {
                    raycastHit = raycastHit2;
                }
            }

            hitInfo = raycastHit;
            return true;
        }

        public static bool RaycastWithExclusion(Vector3 origin, Vector3 dir, out RaycastHit hitInfo, float distance,
            int mask, GameObject[] exclusions) {
            if (exclusions == null) {
                return Physics.Raycast(origin, dir, out hitInfo, distance, mask);
            }

            int num = Physics.RaycastNonAlloc(origin, dir, raycastResults, distance, mask);

            for (int i = 0; i < num; i++) {
                RaycastHit raycastHit = raycastResults[i];
                GameObject gameObject = raycastHit.collider.gameObject;

                if (gameObject != null && !IsInExclusion(raycastHit.collider.gameObject, exclusions)) {
                    hitInfo = raycastHit;
                    return true;
                }
            }

            hitInfo = default;
            return false;
        }

        static bool IsInExclusion(GameObject go, GameObject[] exclusions) {
            for (int i = 0; i < exclusions.Count(); i++) {
                if (go.Equals(exclusions[i])) {
                    return true;
                }
            }

            return false;
        }

        public static void SetGameObjectLayer(GameObject gameObject, int layer) {
            if (gameObject.GetComponent<IgnoreLayerChanges>() == null) {
                gameObject.layer = layer;
            }

            foreach (Transform item in gameObject.transform) {
                SetGameObjectLayer(item.gameObject, layer);
            }
        }
    }
}