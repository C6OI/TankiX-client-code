using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShotValidateSystem : ECSSystem {
        const float ADDITIONAL_GUN_LENGTH = 0.1f;

        readonly HashSet<Type> weaponStates;

        public ShotValidateSystem() => weaponStates = new HashSet<Type> {
            typeof(WeaponUnblockedComponent),
            typeof(WeaponUndergroundComponent),
            typeof(WeaponBlockedComponent)
        };

        [OnEventFire]
        public void Init(NodeAddedEvent evt, ActiveTankNode activeTank, [JoinByTank] [Context] WeaponNode weaponNode) {
            StateUtils.SwitchEntityState<WeaponUnblockedComponent>(weaponNode.Entity, weaponStates);
        }

        [OnEventFire]
        public void ValidateShot(TimeUpdateEvent evt, WeaponMuzzleNode weaponNode, [JoinByTank] TankNode tank) {
            ValidateShot(weaponNode.Entity, new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance), tank, weaponNode.shotValidate);
        }

        [OnEventFire]
        public void ValidateShotBeforeShot(BeforeShotEvent evt, WeaponMuzzleNode weaponNode, [JoinByTank] TankNode tank) {
            ValidateShot(weaponNode.Entity, new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance), tank, weaponNode.shotValidate);
        }

        void ValidateShot(Entity weapon, MuzzleLogicAccessor muzzlePoint, TankNode tank, ShotValidateComponent shotValidate) {
            RaycastHit hitInfo;

            if (IsWeaponUnderground(muzzlePoint, shotValidate.UnderGroundValidateMask, tank, shotValidate.RaycastExclusionGameObjects)) {
                StateUtils.SwitchEntityState<WeaponUndergroundComponent>(weapon, weaponStates);
            } else if (IsWeaponBlocked(muzzlePoint, shotValidate.BlockValidateMask, out hitInfo, shotValidate.RaycastExclusionGameObjects)) {
                AddOrChangeWeaponBlockedComponent(weapon, hitInfo);
            } else {
                StateUtils.SwitchEntityState<WeaponUnblockedComponent>(weapon, weaponStates);
            }
        }

        void AddOrChangeWeaponBlockedComponent(Entity weapon, RaycastHit hitInfo) {
            WeaponBlockedComponent weaponBlockedComponent = !weapon.HasComponent<WeaponBlockedComponent>()
                                                                ? (WeaponBlockedComponent)weapon.CreateNewComponentInstance(typeof(WeaponBlockedComponent))
                                                                : weapon.GetComponent<WeaponBlockedComponent>();

            weaponBlockedComponent.BlockPoint = PhysicsUtil.GetPulledHitPoint(hitInfo);
            weaponBlockedComponent.BlockGameObject = hitInfo.collider.gameObject;
            weaponBlockedComponent.BlockNormal = hitInfo.normal;
            StateUtils.SwitchEntityState(weapon, weaponBlockedComponent, weaponStates);
        }

        bool IsWeaponBlocked(MuzzleLogicAccessor muzzlePoint, int mask, out RaycastHit hitInfo, GameObject[] raycastExclusionGameObjects) {
            Vector3 worldPosition = muzzlePoint.GetWorldPosition();
            Vector3 barrelOriginWorld = muzzlePoint.GetBarrelOriginWorld();
            float distance = (worldPosition - barrelOriginWorld).magnitude + 0.1f;

            if (PhysicsUtil.RaycastWithExclusion(barrelOriginWorld, worldPosition - barrelOriginWorld, out hitInfo, distance, mask, raycastExclusionGameObjects)) {
                return true;
            }

            return false;
        }

        bool IsWeaponUnderground(MuzzleLogicAccessor muzzlePoint, int mask, TankNode tank, GameObject[] raycastExclusionGameObjects) {
            Vector3 barrelOriginWorld = muzzlePoint.GetBarrelOriginWorld();
            Vector3 center = tank.tankColliders.BoundsCollider.bounds.center;
            Vector3 dir = barrelOriginWorld - center;
            RaycastHit hitInfo;

            if (PhysicsUtil.RaycastWithExclusion(center, dir, out hitInfo, dir.magnitude, mask, raycastExclusionGameObjects)) {
                return true;
            }

            return false;
        }

        public class WeaponNode : Node {
            public WeaponComponent weapon;
        }

        public class WeaponMuzzleNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShotValidateComponent shotValidate;

            public TankGroupComponent tankGroup;

            public WeaponComponent weapon;

            public WeaponInstanceComponent weaponInstance;
        }

        public class TankNode : Node {
            public MountPointComponent mountPoint;
            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}