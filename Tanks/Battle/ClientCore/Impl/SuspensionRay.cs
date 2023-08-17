using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class SuspensionRay {
        readonly ChassisComponent chassis;

        readonly ChassisConfigComponent chassisConfig;

        readonly float damping;

        readonly Vector3 direction;

        readonly Vector3 origin;

        public float compression;

        Vector3 globalDirection;

        Vector3 globalOrigin;
        public bool hadPreviousCollision;

        public bool hasCollision;

        public LayerMask layerMask;

        float prevCompression;

        public RaycastHit rayHit;

        public Vector3 surfaceVelocity;

        public Vector3 velocity;

        public SuspensionRay(Rigidbody body, Vector3 origin, Vector3 direction, ChassisConfigComponent chassisConfig,
            ChassisComponent chassis, float damping) {
            rigidbody = body;
            this.origin = origin;
            this.direction = direction;
            this.chassisConfig = chassisConfig;
            this.chassis = chassis;
            this.damping = damping;
            ConvertToGlobal();
            rayHit.distance = chassisConfig.MaxRayLength;
            rayHit.point = globalOrigin + globalDirection * chassisConfig.MaxRayLength;
        }

        public Rigidbody rigidbody { private get; set; }

        public void Update(float dt) {
            Raycast();

            if (hasCollision) {
                ApplySpringForce(dt);
                CalculateSurfaceVelocity();
                velocity = rigidbody.GetPointVelocity(rayHit.point);
            } else {
                surfaceVelocity = Vector3.zero;
                velocity = Vector3.zero;
                rayHit.distance = chassisConfig.MaxRayLength;
                rayHit.point = globalOrigin + globalDirection * chassisConfig.MaxRayLength;
            }
        }

        void Raycast() {
            ConvertToGlobal();
            prevCompression = chassisConfig.MaxRayLength - rayHit.distance;
            hadPreviousCollision = hasCollision;

            hasCollision = Physics.Raycast(new Ray(globalOrigin, globalDirection),
                out rayHit,
                chassisConfig.MaxRayLength,
                layerMask);
        }

        void ConvertToGlobal() {
            globalDirection = rigidbody.transform.TransformDirection(direction);
            globalOrigin = rigidbody.transform.TransformPoint(origin);
        }

        public void ApplySpringForce(float dt) {
            compression = chassisConfig.MaxRayLength - rayHit.distance;
            float num = (compression - prevCompression) * damping / dt;
            float num2 = Mathf.Max(chassis.SpringCoeff * compression + num, 0f);
            rigidbody.AddForceAtPosition(globalDirection * (0f - num2), globalOrigin);
        }

        void CalculateSurfaceVelocity() {
            if (rayHit.rigidbody != null) {
                surfaceVelocity = rayHit.rigidbody.GetPointVelocity(rayHit.point);
            } else {
                surfaceVelocity = Vector3.zero;
            }
        }

        public Vector3 GetGlobalOrigin() => globalOrigin;

        public Vector3 GetGlobalDirection() => globalDirection;
    }
}