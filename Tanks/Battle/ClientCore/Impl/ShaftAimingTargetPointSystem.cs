using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingTargetPointSystem : ECSSystem {
        readonly float EPS_ACTIVE = 0.25f;

        readonly float EPS_INACTIVE = 0.001f;

        [OnEventFire]
        public void CreateTargetPoint(NodeAddedEvent evt, ShaftAimingWorkingStateNode weapon,
            [JoinByTank] SelfTankNode selfTank,
            [JoinAll] ICollection<SingleNode<TankPartIntersectedWithCameraStateComponent>> intersectedTankParts) {
            ShaftAimingWorkingStateComponent shaftAimingWorkingState = weapon.shaftAimingWorkingState;
            bool isInsideTankPart = intersectedTankParts.Count > 1;

            Vector3 barrelOriginWorld =
                new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetBarrelOriginWorld();

            Vector3 workingDirection = weapon.shaftAimingWorkingState.WorkingDirection;
            float workDistance = weapon.verticalSectorsTargeting.WorkDistance;

            Vector3 targetPoint = GetTargetPoint(barrelOriginWorld,
                workingDirection,
                workDistance,
                selfTank.tankColliders.VisualTriggerColliders);

            ShaftAimingTargetPointContainerComponent shaftAimingTargetPointContainerComponent = new();
            shaftAimingTargetPointContainerComponent.Point = targetPoint;
            shaftAimingTargetPointContainerComponent.IsInsideTankPart = isInsideTankPart;
            shaftAimingTargetPointContainerComponent.PrevVerticalAngle = shaftAimingWorkingState.VerticalAngle;
            weapon.Entity.AddComponent(shaftAimingTargetPointContainerComponent);
            ShaftAimingTargetPointComponent shaftAimingTargetPointComponent = new();
            shaftAimingTargetPointComponent.Point = targetPoint;
            shaftAimingTargetPointComponent.IsInsideTankPart = isInsideTankPart;
            weapon.Entity.AddComponent(shaftAimingTargetPointComponent);
        }

        [OnEventFire]
        public void RemoveTargetPoint(NodeRemoveEvent evt, ShaftAimingWorkingStateNode weapon,
            [JoinByUser] SingleNode<SelfUserComponent> selfUser) {
            weapon.Entity.RemoveComponent<ShaftAimingTargetPointContainerComponent>();
            weapon.Entity.RemoveComponent<ShaftAimingTargetPointComponent>();
        }

        [OnEventFire]
        public void CheckTargetPoint(FixedUpdateEvent evt, ShaftAimingTargetPointWorkingStateNode weapon,
            [JoinByTank] SelfTankNode selfTank,
            [JoinAll] ICollection<SingleNode<TankPartIntersectedWithCameraStateComponent>> intersectedTankParts) {
            ShaftAimingTargetPointComponent shaftAimingTargetPoint = weapon.shaftAimingTargetPoint;

            ShaftAimingTargetPointContainerComponent
                shaftAimingTargetPointContainer = weapon.shaftAimingTargetPointContainer;

            ShaftAimingWorkingStateComponent shaftAimingWorkingState = weapon.shaftAimingWorkingState;
            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);
            bool isInsideTankPart = weapon.shaftAimingTargetPoint.IsInsideTankPart;
            bool isInsideTankPart2 = intersectedTankParts.Count > 1;
            Vector3 barrelOriginWorld = muzzleLogicAccessor.GetBarrelOriginWorld();
            float verticalAngle = shaftAimingWorkingState.VerticalAngle;
            Vector3 workingDirection = weapon.shaftAimingWorkingState.WorkingDirection;
            float workDistance = weapon.verticalSectorsTargeting.WorkDistance;
            Vector3 point = weapon.shaftAimingTargetPoint.Point;

            Vector3 targetPoint = GetTargetPoint(barrelOriginWorld,
                workingDirection,
                workDistance,
                selfTank.tankColliders.VisualTriggerColliders);

            shaftAimingTargetPointContainer.Point = targetPoint;
            shaftAimingTargetPointContainer.IsInsideTankPart = isInsideTankPart2;
            float eps = !weapon.shaftAimingWorkingState.IsActive ? EPS_INACTIVE : EPS_ACTIVE;

            CheckTargetPointDiff(point,
                verticalAngle,
                shaftAimingTargetPoint,
                shaftAimingTargetPointContainer,
                isInsideTankPart,
                eps);
        }

        void CheckTargetPointDiff(Vector3 currentPoint, float currentVerticalAngle,
            ShaftAimingTargetPointComponent targetPointComponent,
            ShaftAimingTargetPointContainerComponent targetPointContainerComponent, bool currentIntersectionTankPartStatus,
            float eps) {
            Vector3 point = targetPointContainerComponent.Point;
            bool isInsideTankPart = targetPointContainerComponent.IsInsideTankPart;

            if (currentIntersectionTankPartStatus != isInsideTankPart) {
                UpdateAndShareTargetPoint(targetPointComponent,
                    targetPointContainerComponent,
                    point,
                    isInsideTankPart,
                    currentVerticalAngle);

                return;
            }

            float prevVerticalAngle = targetPointContainerComponent.PrevVerticalAngle;

            if (prevVerticalAngle != currentVerticalAngle && !MathUtil.NearlyEqual(point, currentPoint, eps)) {
                UpdateAndShareTargetPoint(targetPointComponent,
                    targetPointContainerComponent,
                    point,
                    isInsideTankPart,
                    currentVerticalAngle);
            }
        }

        void UpdateAndShareTargetPoint(ShaftAimingTargetPointComponent targetPointComponent,
            ShaftAimingTargetPointContainerComponent targetPointContainerComponent, Vector3 actualPoint,
            bool isInsideTankPart, float currentVerticalAngle) {
            targetPointContainerComponent.PrevVerticalAngle = currentVerticalAngle;
            targetPointComponent.Point = actualPoint;
            targetPointComponent.IsInsideTankPart = isInsideTankPart;
            targetPointComponent.OnChange();
        }

        Vector3 GetTargetPoint(Vector3 start, Vector3 dir, float length, List<GameObject> exclusionList) {
            RaycastExclude raycastExclude = new(exclusionList);

            try {
                RaycastHit hitInfo = default;

                if (Physics.Raycast(start, dir, out hitInfo, length, LayerMasks.VISUAL_TARGETING)) {
                    return hitInfo.point;
                }

                return start + dir * length;
            } finally {
                raycastExclude.Dispose();
            }
        }

        public class ShaftAimingWorkingStateNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public TankGroupComponent tankGroup;

            public VerticalSectorsTargetingComponent verticalSectorsTargeting;

            public WeaponInstanceComponent weaponInstance;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingTargetPointWorkingStateNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShaftAimingTargetPointComponent shaftAimingTargetPoint;

            public ShaftAimingTargetPointContainerComponent shaftAimingTargetPointContainer;

            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public TankGroupComponent tankGroup;

            public VerticalSectorsTargetingComponent verticalSectorsTargeting;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}