using System.Collections.Generic;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientDataStructures.Impl.Cache;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class CameraShaker : MonoBehaviour {
        const float CAMERA_COLLISION_OFFSET = 0.5f;

        [SerializeField] Vector3 defaultPosInfluence = new(0.15f, 0.15f, 0.15f);

        [SerializeField] Vector3 defaultRotInfluence = new(1f, 1f, 1f);

        readonly List<CameraShakeInstance> cameraShakeInstances = new();

        Vector3 posAddShake;

        Vector3 rotAddShake;

        readonly Cache<CameraShakeInstance> shakerInstancesCache = new CacheImpl<CameraShakeInstance>();

        public List<CameraShakeInstance> ShakeInstances => new(cameraShakeInstances);

        void Update() {
            posAddShake = Vector3.zero;
            rotAddShake = Vector3.zero;

            for (int i = 0; i < cameraShakeInstances.Count && i < cameraShakeInstances.Count; i++) {
                CameraShakeInstance cameraShakeInstance = cameraShakeInstances[i];

                if (cameraShakeInstance.CurrentState == CameraShakeState.Inactive && cameraShakeInstance.deleteOnInactive) {
                    cameraShakeInstances.RemoveAt(i);
                    i--;
                    shakerInstancesCache.Free(cameraShakeInstance);
                } else if (cameraShakeInstance.CurrentState != CameraShakeState.Inactive) {
                    posAddShake += MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.positionInfluence);
                    rotAddShake += MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.rotationInfluence);
                }
            }

            transform.SetLocalPositionSafe(CalculateLocalSpaceCameraPosition(posAddShake));
            transform.SetLocalEulerAnglesSafe(rotAddShake);
        }

        Vector3 CalculateLocalSpaceCameraPosition(Vector3 posAddShake) {
            if (cameraShakeInstances.Count == 0) {
                return Vector3.zero;
            }

            Vector3 vector = transform.parent.localToWorldMatrix.MultiplyPoint(posAddShake);
            Vector3 vector2 = transform.parent.localToWorldMatrix.MultiplyPoint(Vector3.zero);
            Vector3 vector3 = vector - vector2;
            float magnitude = vector3.magnitude;
            Vector3 normalized = vector3.normalized;

            if (normalized.sqrMagnitude < 0.5f) {
                return Vector3.zero;
            }

            RaycastHit hitInfo;

            if (Physics.Raycast(new Ray(vector2, normalized), out hitInfo, magnitude + 0.5f, LayerMasks.STATIC)) {
                Vector3 point = hitInfo.point;
                Vector3 vector4 = point - vector2;
                Vector3 v = point - vector4.normalized * Mathf.Min(0.5f, vector4.magnitude);
                return transform.parent.worldToLocalMatrix.MultiplyPoint(v);
            }

            return posAddShake;
        }

        public CameraShakeInstance Shake(CameraShakeInstance shake) {
            cameraShakeInstances.Add(shake);
            return shake;
        }

        public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime) {
            CameraShakeInstance cameraShakeInstance = shakerInstancesCache.GetInstance().Init(magnitude, roughness, fadeInTime, fadeOutTime);
            cameraShakeInstance.positionInfluence = defaultPosInfluence;
            cameraShakeInstance.rotationInfluence = defaultRotInfluence;
            cameraShakeInstances.Add(cameraShakeInstance);
            return cameraShakeInstance;
        }

        public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime, Vector3 posInfluence, Vector3 rotInfluence) {
            CameraShakeInstance cameraShakeInstance = shakerInstancesCache.GetInstance().Init(magnitude, roughness, fadeInTime, fadeOutTime);
            cameraShakeInstance.positionInfluence = posInfluence;
            cameraShakeInstance.rotationInfluence = rotInfluence;
            cameraShakeInstances.Add(cameraShakeInstance);
            return cameraShakeInstance;
        }

        public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime) {
            CameraShakeInstance cameraShakeInstance = shakerInstancesCache.GetInstance().Init(magnitude, roughness);
            cameraShakeInstance.positionInfluence = defaultPosInfluence;
            cameraShakeInstance.rotationInfluence = defaultRotInfluence;
            cameraShakeInstance.StartFadeIn(fadeInTime);
            cameraShakeInstances.Add(cameraShakeInstance);
            return cameraShakeInstance;
        }

        public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime, Vector3 posInfluence, Vector3 rotInfluence) {
            CameraShakeInstance cameraShakeInstance = shakerInstancesCache.GetInstance().Init(magnitude, roughness);
            cameraShakeInstance.positionInfluence = posInfluence;
            cameraShakeInstance.rotationInfluence = rotInfluence;
            cameraShakeInstance.StartFadeIn(fadeInTime);
            cameraShakeInstances.Add(cameraShakeInstance);
            return cameraShakeInstance;
        }

        static Vector3 SmoothDampEuler(Vector3 current, Vector3 target, ref Vector3 velocity, float smoothTime) {
            Vector3 result = default;
            result.x = Mathf.SmoothDampAngle(current.x, target.x, ref velocity.x, smoothTime);
            result.y = Mathf.SmoothDampAngle(current.y, target.y, ref velocity.y, smoothTime);
            result.z = Mathf.SmoothDampAngle(current.z, target.z, ref velocity.z, smoothTime);
            return result;
        }

        static Vector3 MultiplyVectors(Vector3 v, Vector3 w) {
            v.x *= w.x;
            v.y *= w.y;
            v.z *= w.z;
            return v;
        }
    }
}