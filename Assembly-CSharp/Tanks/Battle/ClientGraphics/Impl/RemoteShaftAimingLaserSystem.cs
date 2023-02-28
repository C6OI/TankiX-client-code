using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RemoteShaftAimingLaserSystem : ECSSystem {
        [OnEventFire]
        public void InstantiateLaserForRemoteTank(NodeAddedEvent evt, ShaftAimingLaserNode weapon, [Context] [JoinByTank] RemoteTankNode remoteTank) {
            if ((bool)remoteTank.assembledTank.AssemblyRoot) {
                GameObject asset = weapon.shaftAimingLaser.Asset;
                GameObject gameObject = Object.Instantiate(asset);
                ShaftAimingLaserBehaviour component = gameObject.GetComponent<ShaftAimingLaserBehaviour>();
                weapon.shaftAimingLaser.EffectInstance = component;
                CustomRenderQueue.SetQueue(gameObject, 3150);
                Transform transform = weapon.shaftAimingLaserSource.gameObject.transform;
                gameObject.transform.position = transform.position;
                gameObject.transform.rotation = transform.rotation;
                component.Init();
                component.SetColor(weapon.shaftAimingColorEffect.ChoosenColor);
                weapon.Entity.AddComponent<ShaftAimingLaserReadyComponent>();
            }
        }

        [OnEventFire]
        public void ShowAndInitLaser(NodeAddedEvent evt, AimingLaserTargetPointNode weapon, [Context] [JoinByTank] ActiveTankNode tank, [JoinAll] [Context] CameraNode camera) {
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetBarrelOriginWorld();
            Vector3 point = weapon.shaftAimingTargetPoint.Point;
            Vector3 laserDir = Vector3.Normalize(point - barrelOriginWorld);
            ShaftAimingLaserBehaviour shaftAimingLaserBehaviour = InterpolateLaser(weapon, barrelOriginWorld, laserDir);
            shaftAimingLaserBehaviour.Show();
        }

        [OnEventFire]
        public void UpdateLaserTargetPosition(UpdateEvent evt, AimingLaserTargetPointNode weapon, [JoinByTank] RemoteTankNode tank, [JoinAll] CameraNode camera) {
            Vector3 point = weapon.shaftAimingTargetPoint.Point;
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetBarrelOriginWorld();
            Vector3 currentLaserDirection = weapon.shaftAimingLaser.CurrentLaserDirection;
            Vector3 b = Vector3.Normalize(point - barrelOriginWorld);
            Vector3 normalized = Vector3.Lerp(currentLaserDirection, b, weapon.shaftAimingLaser.InterpolationCoeff).normalized;
            InterpolateLaser(weapon, barrelOriginWorld, normalized);
        }

        [OnEventFire]
        public void HideLaser(NodeRemoveEvent evt, AimingLaserTargetPointNode weapon) {
            weapon.shaftAimingLaser.EffectInstance.Hide();
        }

        [OnEventFire]
        public void CleanLaserOnTankDeath(NodeRemoveEvent evt, AimingReadyLaserForNRNode nr, [JoinSelf] AimingReadyLaserNode weapon) {
            foreach (ShaftAimingLaserBehaviour effectInstance in weapon.shaftAimingLaser.EffectInstances) {
                effectInstance.Kill();
            }

            weapon.Entity.RemoveComponent<ShaftAimingLaserReadyComponent>();
        }

        ShaftAimingLaserBehaviour InterpolateLaser(AimingLaserTargetPointNode weapon, Vector3 startPosition, Vector3 laserDir) {
            Vector3 leftDirectionWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetLeftDirectionWorld();
            laserDir = Vector3.ProjectOnPlane(laserDir, leftDirectionWorld).normalized;
            ShaftAimingLaserBehaviour effectInstance = weapon.shaftAimingLaser.EffectInstance;
            float maxLength = weapon.shaftAimingLaser.MaxLength;
            float minLength = weapon.shaftAimingLaser.MinLength;
            DirectionData directionData = weapon.targetCollector.Collect(startPosition, laserDir, maxLength, LayerMasks.VISUAL_TARGETING);
            Vector3 targetPosition;
            bool flag;
            bool flag2;

            if (directionData.HasAnyHit()) {
                flag = true;
                flag2 = directionData.FirstAnyHitDistance() >= minLength;
                targetPosition = directionData.FirstAnyHitPosition();
            } else {
                flag = false;
                flag2 = true;
                targetPosition = startPosition + laserDir * maxLength;
            }

            bool flag3 = !weapon.shaftAimingTargetPoint.IsInsideTankPart;
            flag = flag && flag3;
            flag2 = flag2 && flag3;
            Vector3 position = weapon.shaftAimingLaserSource.transform.position;
            effectInstance.UpdateTargetPosition(position, targetPosition, flag2, flag);
            weapon.shaftAimingLaser.CurrentLaserDirection = laserDir;
            return effectInstance;
        }

        public class CameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;
        }

        public class RemoteTankNode : Node {
            public AssembledTankComponent assembledTank;

            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public RemoteTankComponent remoteTank;

            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class ActiveTankNode : RemoteTankNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class ShaftAimingLaserNode : Node {
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;

            public ShaftAimingColorEffectPreparedComponent shaftAimingColorEffectPrepared;
            public ShaftAimingLaserComponent shaftAimingLaser;

            public ShaftAimingLaserSourceComponent shaftAimingLaserSource;

            public TankGroupComponent tankGroup;
        }

        public class AimingReadyLaserNode : ShaftAimingLaserNode {
            public ShaftAimingLaserReadyComponent shaftAimingLaserReady;
        }

        public class AimingLaserTargetPointNode : AimingReadyLaserNode {
            public MuzzlePointComponent muzzlePoint;

            public ShaftAimingTargetPointComponent shaftAimingTargetPoint;

            public TargetCollectorComponent targetCollector;

            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class AimingReadyLaserForNRNode : Node {
            public ShaftAimingLaserComponent shaftAimingLaser;

            public ShaftAimingLaserSourceComponent shaftAimingLaserSource;

            public TankGroupComponent tankGroup;
        }
    }
}