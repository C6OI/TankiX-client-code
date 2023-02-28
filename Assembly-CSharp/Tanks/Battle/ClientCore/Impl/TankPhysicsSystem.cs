using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankPhysicsSystem : ECSSystem {
        static readonly float MAX_DEPENETRATION_VELOCITY = 5f;

        [OnEventFire]
        public void AttachCollidersToTank(NodeAddedEvent e, PhysicsNode node) {
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            BoxCollider boundsCollider = node.tankCollidersUnity.GetBoundsCollider();
            SetInertiaTensor(ref rigidbody, boundsCollider.size, boundsCollider.center);
            node.Entity.AddComponent(new TriggerObjectComponent(boundsCollider.gameObject));
            rigidbody.maxDepenetrationVelocity = MAX_DEPENETRATION_VELOCITY;
            node.rigidbody.Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rigidbody.WakeUp();
        }

        [OnEventFire]
        public void WakeUpActiveTankBody(NodeAddedEvent e, TankActiveStateNode node) {
            node.rigidbody.Rigidbody.WakeUp();
        }

        [OnEventFire]
        public void SetHighFriction(NodeAddedEvent e, DeadTankNode tank) {
            List<Collider> tankToStaticColliders = tank.tankColliders.TankToStaticColliders;
            int count = tankToStaticColliders.Count;

            for (int i = 0; i < count; i++) {
                Collider collider = tankToStaticColliders[i];
                collider.material = tank.tankCollidersUnity.highFrictionMaterial;
            }
        }

        [OnEventFire]
        public void SetLowFriction(NodeRemoveEvent e, DeadTankNode tank) {
            List<Collider> tankToStaticColliders = tank.tankColliders.TankToStaticColliders;
            int count = tankToStaticColliders.Count;

            for (int i = 0; i < count; i++) {
                Collider collider = tankToStaticColliders[i];
                collider.material = tank.tankCollidersUnity.lowFrictionMaterial;
            }

            tank.tankColliders.TankToStaticTopCollider.material = tank.tankCollidersUnity.highFrictionMaterial;
        }

        void SetInertiaTensor(ref Rigidbody rigidbody, Vector3 size, Vector3 center) {
            rigidbody.centerOfMass = center;
            rigidbody.inertiaTensor = new Vector3(size.y * size.y + size.z * size.z, size.x * size.x + size.z * size.z, size.y * size.y + size.x * size.x) * (rigidbody.mass / 12f);
            rigidbody.inertiaTensorRotation = Quaternion.identity;
        }

        [OnEventFire]
        public void ApplyDepenetrationForce(FixedUpdateEvent evt, SelfPhysicNode selfPhysics, [JoinByBattle] ICollection<RemotePhysicNode> remotePhysics) {
            Rigidbody rigidbody = selfPhysics.rigidbody.Rigidbody;
            BoxCollider boxCollider = (BoxCollider)selfPhysics.tankColliders.TankToTankCollider;
            bool flag = false;

            foreach (RemotePhysicNode remotePhysic in remotePhysics) {
                Rigidbody rigidbody2 = remotePhysic.rigidbody.Rigidbody;
                BoxCollider boxCollider2 = (BoxCollider)remotePhysic.tankColliders.TankToTankCollider;

                if (boxCollider.bounds.Intersects(boxCollider2.bounds)) {
                    Entity entity = remotePhysic.Entity;

                    if ((entity.HasComponent<TankActiveStateComponent>() || entity.HasComponent<TankDeadStateComponent>()) &&
                        DepenetrationForce.ApplyDepenetrationForce(rigidbody, boxCollider, rigidbody2, boxCollider2)) {
                        flag = true;
                    }
                }
            }

            if (flag) {
                rigidbody.maxDepenetrationVelocity = 0f;
            } else {
                rigidbody.maxDepenetrationVelocity = MAX_DEPENETRATION_VELOCITY;
            }
        }

        [OnEventFire]
        public void ApplyWingForces(FixedUpdateEvent evt, PhysicNode selfPhysics, [JoinAll] MapNode mapPhysic) {
            Rigidbody rigidbody = selfPhysics.rigidbody.Rigidbody;
            Transform rigidbodyTransform = selfPhysics.rigidbody.RigidbodyTransform;

            if (rigidbody == null) {
                return;
            }

            float num = Vector3.Dot(rigidbodyTransform.up, Vector3.up);

            if (num >= 0.9f) {
                return;
            }

            float num2 = Physics.gravity.y / -10f;
            Vector3 normalized = rigidbodyTransform.InverseTransformDirection(rigidbody.angularVelocity).normalized;
            float num3 = 0.5f;

            if (num <= 0f) {
                if (num3 > 0f && rigidbody.angularVelocity.magnitude > 0.2f) {
                    float num4 = num3 * rigidbody.mass * num2 * Mathf.Sign(normalized.z);
                    rigidbody.AddRelativeTorqueSafe(new Vector3(0f, 0f, 1f) * num4);
                }

                return;
            }

            float num5 = 4f * rigidbody.mass * num2;

            if (!ApplyWingForce(rigidbody, rigidbodyTransform.right, new Vector3(0f, 0f, 1f) * num5)) {
                ApplyWingForce(rigidbody, -rigidbodyTransform.right, new Vector3(0f, 0f, -1f) * num5);
            }

            float num6 = 4f * rigidbody.mass * num2;

            if (!ApplyWingForce(rigidbody, rigidbodyTransform.forward, new Vector3(-1f, 0f, 0f) * num6)) {
                ApplyWingForce(rigidbody, -rigidbodyTransform.forward, new Vector3(1f, 0f, 0f) * num6);
            }
        }

        public bool ApplyWingForce(Rigidbody body, Vector3 wingDirection, Vector3 strength) {
            float num = 0f - Vector3.Dot(wingDirection, Vector3.up);

            if (num <= 0f) {
                return false;
            }

            body.AddRelativeTorqueSafe(strength * num * num);
            return true;
        }

        [OnEventFire]
        public void ApplyMoveHelperForces(FixedUpdateEvent evt, PhysicNode selfPhysics) {
            Rigidbody rigidbody = selfPhysics.rigidbody.Rigidbody;
            Transform rigidbodyTransform = selfPhysics.rigidbody.RigidbodyTransform;

            if (rigidbody == null) {
                return;
            }

            BoxCollider boxCollider = (BoxCollider)selfPhysics.tankColliders.TankToTankCollider;
            Vector3 vector = boxCollider.size * 0.5f;
            Vector3 vector2 = rigidbodyTransform.TransformPoint(new Vector3(0f - vector.x, vector.y * 0.5f, vector.z));
            Vector3 vector3 = rigidbodyTransform.TransformPoint(new Vector3(vector.x, vector.y * 0.5f, vector.z));
            float magnitude = Vector3.Project(rigidbody.velocity, rigidbodyTransform.forward).magnitude;
            float num = Vector3.Dot(rigidbody.velocity.normalized, rigidbodyTransform.forward);
            float num2 = 0.1f + magnitude * 0.25f;
            bool flag = StaticCollided(vector2, rigidbodyTransform.forward, num2);
            bool flag2 = !StaticCollided(vector2 + rigidbodyTransform.right * 0.3f, rigidbodyTransform.forward, num2 * 1.2f);
            bool flag3 = StaticCollided(vector3, rigidbodyTransform.forward, num2);
            bool flag4 = !StaticCollided(vector3 - rigidbodyTransform.right * 0.3f, rigidbodyTransform.forward, num2 * 1.2f);

            if (num > 0f) {
                if (flag && flag2 && !flag3) {
                    rigidbody.AddTorqueSafe(new Vector3(0f, 1f, 0f) * magnitude * rigidbody.mass * 2f);
                }

                if (!flag && flag4 && flag3) {
                    rigidbody.AddTorqueSafe(-new Vector3(0f, 1f, 0f) * magnitude * rigidbody.mass * 2f);
                }
            }
        }

        bool StaticCollided(Vector3 origin, Vector3 dir, float length) {
            RaycastHit hitInfo;
            return Physics.Raycast(origin, dir, out hitInfo, length, LayerMasks.STATIC) && Mathf.Abs(hitInfo.normal.normalized.y) < 0.3f;
        }

        public class PhysicsNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public ChassisConfigComponent chassisConfig;
            public RigidbodyComponent rigidbody;

            public TankCollidersUnityComponent tankCollidersUnity;
        }

        public class DeadTankNode : Node {
            public TankComponent tank;

            public TankCollidersComponent tankColliders;

            public TankCollidersUnityComponent tankCollidersUnity;

            public TankDeadStateComponent tankDeadState;
        }

        public class TankActiveStateNode : Node {
            public RigidbodyComponent rigidbody;

            public SelfTankComponent selfTank;
            public TankActiveStateComponent tankActiveState;
        }

        public class SelfPhysicNode : Node {
            public RigidbodyComponent rigidbody;
            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;

            public TankCollidersComponent tankColliders;
        }

        public class PhysicNode : Node {
            public ChassisComponent chassis;

            public RigidbodyComponent rigidbody;
            public TankActiveStateComponent tankActiveState;

            public TankCollidersComponent tankColliders;
        }

        public class RemotePhysicNode : Node {
            public RemoteTankComponent remoteTank;

            public RigidbodyComponent rigidbody;

            public TankCollidersComponent tankColliders;
        }

        public class MapNode : Node {
            public MapInstanceComponent mapInstance;
        }
    }
}