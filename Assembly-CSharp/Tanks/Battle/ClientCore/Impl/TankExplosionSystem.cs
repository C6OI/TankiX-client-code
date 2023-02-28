using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankExplosionSystem : ECSSystem {
        [OnEventFire]
        public void Reset(NodeAddedEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] SingleNode<DetachedWeaponComponent> weapon) {
            weapon.Entity.RemoveComponent<DetachedWeaponComponent>();
        }

        [OnEventFire]
        public void DeathJump(SelfTankExplosionEvent e, SelfTankNode tank) {
            HullDeathJump(tank.rigidbody.Rigidbody);
            ScheduleEvent<SendTankMovementEvent>(tank);
            ScheduleEvent<DetachWeaponIfPossibleEvent>(tank);
        }

        [OnEventFire]
        public void SelfDetachWeapon(DetachWeaponIfPossibleEvent e, TankNode tank, [JoinByTank] UnblockedWeaponNode weapon) {
            Rigidbody rigidbody = DetachWeapon(tank, weapon);
            rigidbody.SetVelocitySafe(rigidbody.velocity + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(2f, 3f), Random.Range(-1.5f, 1.5f)));

            rigidbody.SetAngularVelocitySafe(rigidbody.angularVelocity +
                                             Vector3.Scale(Random.onUnitSphere, new Vector3(Random.Range(2f, 4f), Random.Range(0.5f, 1f), Random.Range(2f, 4f))));

            ScheduleEvent(new DetachWeaponEvent(rigidbody.velocity, rigidbody.angularVelocity), tank);
        }

        [OnEventFire]
        public void RemoteDetachWeapon(DetachWeaponEvent e, RemoteTankNode tank, [JoinByTank] UnblockedWeaponNode weapon) {
            Rigidbody rigidbody = DetachWeapon(tank, weapon);
            rigidbody.SetVelocitySafe(e.Velocity);
            rigidbody.SetAngularVelocitySafe(e.AngularVelocity);
        }

        [OnEventFire]
        public void AttachDetachedWeapon(NodeRemoveEvent e, TankNode tank, [JoinByTank] [Context] TankIncarnationNode incornation, [JoinByTank] DetachedWeaponNode weapon) {
            StopCollide(weapon.weaponDetachCollider);
            WeaponVisualRootComponent weaponVisualRoot = weapon.weaponVisualRoot;
            weaponVisualRoot.transform.SetParent(tank.visualMountPoint.MountPoint, false);
            weaponVisualRoot.transform.localRotation = Quaternion.identity;
            weaponVisualRoot.transform.localPosition = Vector3.zero;
            StopCollide(weapon.weaponDetachCollider);
            MeshCollider visualTriggerMeshCollider = weaponVisualRoot.VisualTriggerMarker.VisualTriggerMeshCollider;
            visualTriggerMeshCollider.enabled = true;
            weapon.Entity.RemoveComponentIfPresent<DetachedWeaponComponent>();
        }

        [OnEventFire]
        public void StopCollide(NodeAddedEvent e, TransparentDetachedWeapon weapon) {
            StopCollide(weapon.weaponDetachCollider);
        }

        public void StopCollide(WeaponDetachColliderComponent colliderComponent) {
            colliderComponent.Collider.enabled = false;
            Rigidbody rigidbody = colliderComponent.Rigidbody;
            rigidbody.isKinematic = true;
            rigidbody.detectCollisions = false;
        }

        void HullDeathJump(Rigidbody hullRigidbody) {
            hullRigidbody.SetVelocitySafe(hullRigidbody.velocity += new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 3f), Random.Range(-1f, 1f)));

            hullRigidbody.SetAngularVelocitySafe(hullRigidbody.angularVelocity +
                                                 Vector3.Scale(Random.onUnitSphere, new Vector3(Random.Range(1f, 3f), Random.Range(0.5f, 1f), Random.Range(1f, 3f))));
        }

        Rigidbody DetachWeapon(TankNode tank, UnblockedWeaponNode weapon) {
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            WeaponVisualRootComponent weaponVisualRoot = weapon.weaponVisualRoot;
            WeaponDetachColliderComponent weaponDetachCollider = weapon.weaponDetachCollider;
            GameObject gameObject = weapon.weaponVisualRoot.gameObject;

            if (weapon.Entity.HasComponent<DetachedWeaponComponent>()) {
                return rigidbody;
            }

            gameObject.transform.parent = tank.assembledTank.AssemblyRoot.transform;
            gameObject.layer = Layers.TANK_AND_STATIC;
            MeshCollider visualTriggerMeshCollider = weaponVisualRoot.VisualTriggerMarker.VisualTriggerMeshCollider;
            visualTriggerMeshCollider.enabled = false;
            MeshCollider collider = weaponDetachCollider.Collider;
            collider.enabled = true;
            Rigidbody rigidbody2 = weaponDetachCollider.Rigidbody;
            Bounds bounds = collider.sharedMesh.bounds;
            Vector3 center = bounds.center;
            center.z = 0f;
            SetInertiaTensor(rigidbody2, bounds.size, center);
            rigidbody2.mass = rigidbody.mass / 10f;
            rigidbody2.maxAngularVelocity = rigidbody.maxAngularVelocity;
            rigidbody2.maxDepenetrationVelocity = 0f;
            rigidbody2.angularDrag = 2f;
            rigidbody2.drag = 0f;
            rigidbody2.SetVelocitySafe(rigidbody.velocity);
            rigidbody2.SetAngularVelocitySafe(rigidbody.angularVelocity);
            rigidbody2.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody2.isKinematic = false;
            rigidbody2.detectCollisions = true;
            weapon.Entity.AddComponent<DetachedWeaponComponent>();
            return rigidbody2;
        }

        void SetInertiaTensor(Rigidbody rigidbody, Vector3 size, Vector3 center) {
            rigidbody.centerOfMass = center;
            float y = size.y;
            float z = size.z;
            float x = size.x;
            float num = y * y;
            float num2 = x * x;
            float num3 = z * z;
            rigidbody.inertiaTensor = new Vector3(num + num3, num2 + num3, num + num2) * (rigidbody.mass / 12f);
            rigidbody.inertiaTensorRotation = Quaternion.identity;
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;

            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public RigidbodyComponent rigidbody;
            public TankComponent tank;

            public TankCollidersUnityComponent tankCollidersUnity;

            public TankGroupComponent tankGroup;

            public TankVisualRootComponent tankVisualRoot;

            public TrackComponent track;

            public VisualMountPointComponent visualMountPoint;
        }

        public class SelfTankNode : TankNode {
            public SelfComponent self;
        }

        public class RemoteTankNode : TankNode {
            public RemoteTankComponent remoteTank;
        }

        public class UnblockedWeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponDetachColliderComponent weaponDetachCollider;

            public WeaponUnblockedComponent weaponUnblocked;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class TransparentDetachedWeapon : Node {
            public DetachedWeaponComponent detachedWeapon;
            public TankGroupComponent tankGroup;

            public TransparencyTransitionComponent transparencyTransition;

            public WeaponDetachColliderComponent weaponDetachCollider;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class DetachedWeaponNode : Node {
            public DetachedWeaponComponent detachedWeapon;
            public TankGroupComponent tankGroup;

            public WeaponDetachColliderComponent weaponDetachCollider;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }
    }
}