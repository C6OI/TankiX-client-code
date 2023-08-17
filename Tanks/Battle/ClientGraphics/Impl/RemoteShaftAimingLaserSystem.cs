using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RemoteShaftAimingLaserSystem : ECSSystem {
        [OnEventFire]
        public void InstantiateLaserForRemoteTank(NodeAddedEvent evt, ShaftAimingLaserNode weapon,
            [JoinByTank] [Context] RemoteTankNode remoteTank) {
            GameObject asset = weapon.shaftAimingLaser.Asset;
            GameObject gameObject = Object.Instantiate(asset);
            weapon.shaftAimingLaser.EffectInstance = gameObject;
            Transform transform = weapon.shaftAimingLaserSource.gameObject.transform;
            gameObject.transform.position = transform.position;
            gameObject.transform.rotation = transform.rotation;
            ShaftAimingLaserBehaviour component = gameObject.GetComponent<ShaftAimingLaserBehaviour>();
            component.Init();
            component.SetColor(weapon.shaftAimingColorEffect.ChoosenColor);
            weapon.Entity.AddComponent<ShaftAimingLaserReadyComponent>();
        }

        [OnEventFire]
        public void ShowAndInitLaser(NodeAddedEvent evt, AimingLaserTargetPointNode weapon,
            [JoinByTank] [Context] TankCollidersNode tank, [JoinAll] [Context] CameraNode camera) {
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetBarrelOriginWorld();
            Vector3 point = weapon.shaftAimingTargetPoint.Point;
            Vector3 laserDir = Vector3.Normalize(point - barrelOriginWorld);

            ShaftAimingLaserBehaviour shaftAimingLaserBehaviour = InterpolateLaser(weapon,
                barrelOriginWorld,
                laserDir,
                tank.tankColliders.VisualTriggerColliders);

            shaftAimingLaserBehaviour.Show();
        }

        [OnEventFire]
        public void UpdateLaserTargetPosition(UpdateEvent evt, AimingLaserTargetPointNode weapon,
            [JoinByTank] TankCollidersNode tank, [JoinAll] CameraNode camera) {
            Vector3 point = weapon.shaftAimingTargetPoint.Point;
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetBarrelOriginWorld();
            Vector3 currentLaserDirection = weapon.shaftAimingLaser.CurrentLaserDirection;
            Vector3 b = Vector3.Normalize(point - barrelOriginWorld);

            Vector3 normalized = Vector3.Lerp(currentLaserDirection, b, weapon.shaftAimingLaser.InterpolationCoeff)
                .normalized;

            InterpolateLaser(weapon, barrelOriginWorld, normalized, tank.tankColliders.VisualTriggerColliders);
        }

        [OnEventFire]
        public void HideLaser(NodeRemoveEvent evt, AimingLaserTargetPointNode weapon) => weapon.shaftAimingLaser
            .EffectInstance.GetComponent<ShaftAimingLaserBehaviour>().Hide();

        [OnEventFire]
        public void CleanLaserOnTankDeath(NodeRemoveEvent evt, AimingReadyLaserForNRNode nr,
            [JoinSelf] AimingReadyLaserNode weapon) {
            weapon.shaftAimingLaser.EffectInstance.GetComponent<ShaftAimingLaserBehaviour>().Kill();
            weapon.Entity.RemoveComponent<ShaftAimingLaserReadyComponent>();
        }

        ShaftAimingLaserBehaviour InterpolateLaser(AimingLaserTargetPointNode weapon, Vector3 startPosition,
            Vector3 laserDir, List<GameObject> raycastExclude) {
            Vector3 leftDirectionWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetLeftDirectionWorld();
            laserDir = Vector3.ProjectOnPlane(laserDir, leftDirectionWorld).normalized;
            RaycastExclude raycastExclude2 = new(raycastExclude);

            try {
                ShaftAimingLaserBehaviour component =
                    weapon.shaftAimingLaser.EffectInstance.GetComponent<ShaftAimingLaserBehaviour>();

                float maxLength = weapon.shaftAimingLaser.MaxLength;
                float minLength = weapon.shaftAimingLaser.MinLength;
                RaycastHit hitInfo = default;
                Vector3 zero = Vector3.zero;
                bool flag;
                bool flag2;

                if (Physics.Raycast(startPosition, laserDir, out hitInfo, maxLength, LayerMasks.VISUAL_TARGETING)) {
                    flag = true;
                    flag2 = hitInfo.distance >= minLength;
                    zero = hitInfo.point;
                } else {
                    flag = false;
                    flag2 = true;
                    zero = startPosition + laserDir * maxLength;
                }

                bool flag3 = !weapon.shaftAimingTargetPoint.IsInsideTankPart;
                flag = flag && flag3;
                flag2 = flag2 && flag3;
                Vector3 position = weapon.shaftAimingLaserSource.transform.position;
                component.UpdateTargetPosition(position, zero, flag2, flag);
                weapon.shaftAimingLaser.CurrentLaserDirection = laserDir;
                return component;
            } finally {
                raycastExclude2.Dispose();
            }
        }

        public class CameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;
        }

        public class RemoteTankNode : Node {
            public RemoteTankComponent remoteTank;

            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingLaserNode : Node {
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;

            public ShaftAimingColorEffectPreparedComponent shaftAimingColorEffectPrepared;
            public ShaftAimingLaserComponent shaftAimingLaser;

            public ShaftAimingLaserSourceComponent shaftAimingLaserSource;

            public TankGroupComponent tankGroup;
        }

        public class AimingLaserTargetPointNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShaftAimingLaserComponent shaftAimingLaser;

            public ShaftAimingLaserReadyComponent shaftAimingLaserReady;

            public ShaftAimingLaserSourceComponent shaftAimingLaserSource;

            public ShaftAimingTargetPointComponent shaftAimingTargetPoint;

            public TankGroupComponent tankGroup;

            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class TankCollidersNode : Node {
            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class AimingReadyLaserNode : Node {
            public ShaftAimingLaserComponent shaftAimingLaser;

            public ShaftAimingLaserReadyComponent shaftAimingLaserReady;

            public ShaftAimingLaserSourceComponent shaftAimingLaserSource;

            public TankGroupComponent tankGroup;
        }

        public class AimingReadyLaserForNRNode : Node {
            public ShaftAimingLaserComponent shaftAimingLaser;

            public ShaftAimingLaserSourceComponent shaftAimingLaserSource;

            public TankGroupComponent tankGroup;
        }
    }
}