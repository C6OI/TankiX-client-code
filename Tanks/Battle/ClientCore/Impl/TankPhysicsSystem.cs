using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankPhysicsSystem : ECSSystem {
        [OnEventFire]
        public void AttachCollidersToTank(NodeAddedEvent e, PhysicsNode node) {
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            BoxCollider boundsCollider = node.tankCollidersUnity.GetBoundsCollider();
            SetInertiaTensor(ref rigidbody, boundsCollider.size, boundsCollider.center);
            node.Entity.AddComponent(new TriggerObjectComponent(boundsCollider.gameObject));
            rigidbody.maxDepenetrationVelocity = 5f;
            rigidbody.WakeUp();
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

        void SetInertiaTensor(ref Rigidbody rigidbody, Vector3 size, Vector3 center) {
            rigidbody.centerOfMass = center;

            rigidbody.inertiaTensor =
                new Vector3(size.y * size.y + size.z * size.z,
                    size.x * size.x + size.z * size.z,
                    size.y * size.y + size.x * size.x) *
                (rigidbody.mass / 12f);

            rigidbody.inertiaTensorRotation = Quaternion.identity;
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
    }
}