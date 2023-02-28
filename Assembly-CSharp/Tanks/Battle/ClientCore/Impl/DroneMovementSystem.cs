using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class DroneMovementSystem : ECSSystem {
        const float droneLookDownAngle = 0.5f;

        [OnEventFire]
        public void DroneMovement(FixedUpdateEvent e, DroneNode drone) {
            DroneOwnerComponent droneOwner = drone.droneOwner;
            Rigidbody rigidbody = drone.rigidbody.Rigidbody;
            DroneMoveConfigComponent droneMoveConfig = drone.droneMoveConfig;

            if (droneOwner.Incarnation.Alive && (bool)droneOwner.Rigidbody) {
                ApplyMovingForce(rigidbody, droneMoveConfig, droneOwner);
            }

            Rigidbody rigidbody2 = drone.rigidbody.Rigidbody;
            ApplyStabilization(rigidbody2);
            Rigidbody rigidbody3 = null;

            if (drone.Entity.HasComponent<UnitTargetComponent>()) {
                Entity target = drone.Entity.GetComponent<UnitTargetComponent>().Target;

                if (target.HasComponent<RigidbodyComponent>()) {
                    rigidbody3 = target.GetComponent<RigidbodyComponent>().Rigidbody;
                }
            }

            if (rigidbody3 != null) {
                Vector3 position = rigidbody3.position;
                position.y += 0.5f;
                Vector3 normalized = (position - rigidbody2.position).normalized;
                ApplyTargetingForce(rigidbody2, normalized);
            } else if (droneOwner.Incarnation.Alive && (bool)droneOwner.Rigidbody) {
                Vector3 forward = droneOwner.Rigidbody.transform.forward;
                forward = (forward - Vector3.up * 0.5f).normalized;
                ApplyTargetingForce(rigidbody2, forward);
            }
        }

        void ApplyMovingForce(Rigidbody body, DroneMoveConfigComponent config, DroneOwnerComponent owner) {
            Transform transform = owner.Rigidbody.transform;
            Vector3 vector = transform.position + config.FlyPosition;
            Vector3 position = body.position;
            Vector3 vector2 = vector - position;
            float magnitude = vector2.magnitude;
            Vector3 vector3 = vector2 / magnitude;

            if (magnitude > 1f) {
                float num = Mathf.Clamp(magnitude * config.Acceleration, 0f, config.MoveSpeed);
                body.AddForceSafe(vector3 * num);
            }
        }

        void ApplyTargetingForce(Rigidbody body, Vector3 targetingDirection) {
            float num = Vector3.Dot(targetingDirection, body.transform.forward);
            float f = Vector3.Dot(targetingDirection, body.transform.right);
            body.AddTorqueSafe(0f, Mathf.Sign(f) * 4f * body.mass * (1.5f - num), 0f);
            float f2 = Vector3.Dot(targetingDirection, -body.transform.up);
            float x = Mathf.Clamp(Mathf.Sign(f2) * 4f * body.mass * (1.5f - num), -2f, 4f);
            body.AddRelativeTorque(x, 0f, 0f);
        }

        public void ApplyStabilization(Rigidbody body) {
            float num = 5f;
            float num2 = num * body.mass;

            if (!ApplyWingForce(body, body.transform.right, new Vector3(0f, 0f, 1f).normalized * num2)) {
                ApplyWingForce(body, -body.transform.right, new Vector3(0f, 0f, -1f).normalized * num2);
            }

            num = 3f;
            float num3 = num * body.mass;

            if (!ApplyWingForce(body, body.transform.forward, new Vector3(-1f, 0f, 0f).normalized * num3)) {
                ApplyWingForce(body, -body.transform.forward, new Vector3(1f, 0f, 0f).normalized * num3);
            }
        }

        public bool ApplyWingForce(Rigidbody body, Vector3 wingDirection, Vector3 strength) {
            float num = 0f - Vector3.Dot(wingDirection, Vector3.up);

            if (num <= 0f) {
                return false;
            }

            body.AddRelativeTorqueSafe(strength * num);
            return true;
        }

        public class DroneNode : Node {
            public DroneEffectComponent droneEffect;

            public DroneMoveConfigComponent droneMoveConfig;

            public DroneOwnerComponent droneOwner;

            public RigidbodyComponent rigidbody;

            public UnitGroupComponent unitGroup;

            public UnitMoveComponent unitMove;
        }
    }
}