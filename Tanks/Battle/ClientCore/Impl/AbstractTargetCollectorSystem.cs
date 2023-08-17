using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class AbstractTargetCollectorSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        protected void CollectTargetsWithExclusion(TargetingData targetingData, IEnumerable<GameObject> exclusionObjects,
            Func<Entity, RaycastHit, bool, bool> targetValidator,
            Func<RaycastHit, GameObject, GameObject, Vector3> calculateLocalHitPoint, int mask,
            bool isInsideAttack = false) {
            RaycastExclude raycastExclude = new(exclusionObjects);

            try {
                CollectTargets(targetingData, targetValidator, calculateLocalHitPoint, mask, isInsideAttack);
            } finally {
                raycastExclude.Dispose();
            }
        }

        protected void CollectTargets(TargetingData targetingData, Func<Entity, RaycastHit, bool, bool> targetValidator,
            Func<RaycastHit, GameObject, GameObject, Vector3> calculateLocalHitPoint, int mask,
            bool isInsideAttack = false) {
            foreach (DirectionData direction in targetingData.Directions) {
                FindTargetOnDirection(targetingData,
                    direction,
                    targetValidator,
                    calculateLocalHitPoint,
                    mask,
                    isInsideAttack);
            }
        }

        protected void FindTargetOnDirection(TargetingData targetingData, DirectionData direction,
            Func<Entity, RaycastHit, bool, bool> targetValidator,
            Func<RaycastHit, GameObject, GameObject, Vector3> calculateLocalHitPoint, int mask,
            bool isInsideAttack = false) {
            Vector3 origin = direction.Origin;
            Vector3 dir = direction.Dir;
            RaycastHit hitInfo;

            if (PhysicsUtil.InsiderRaycast(origin, dir, out hitInfo, targetingData.FullDistance, mask, isInsideAttack)) {
                GameObject gameObject = hitInfo.transform.gameObject;
                TargetBehaviour componentInParent = gameObject.GetComponentInParent<TargetBehaviour>();

                if (componentInParent == null || !targetValidator(componentInParent.Entity, hitInfo, isInsideAttack)) {
                    direction.StaticHit = new StaticHit {
                        Position = PhysicsUtil.GetPulledHitPoint(hitInfo),
                        Normal = hitInfo.normal
                    };

                    return;
                }

                Color color = !direction.Extra ? Color.red : Color.magenta;
                Entity entity = componentInParent.Entity;

                GameObject gameObject2 =
                    componentInParent.gameObject.GetComponentInChildren<PhysicalRootBehaviour>().gameObject;

                GameObject gameObject3 = componentInParent.gameObject.GetComponentInChildren<TankVisualRootComponent>()
                    .gameObject;

                Vector3 vector = hitInfo.point - origin;
                TargetData targetData = BattleCache.targetData.GetInstance().Init(entity);
                targetData.HitPoint = hitInfo.point;
                targetData.LocalHitPoint = calculateLocalHitPoint(hitInfo, gameObject2, gameObject3);
                targetData.TargetPosition = gameObject2.transform.position;
                targetData.HitDirection = vector.normalized;
                targetData.HitDistance = vector.magnitude;
                direction.Targets.Add(targetData);
            }
        }

        protected static bool CheckTargetCollision(RaycastHit hitInfo, int targetCollisionLayerMask) {
            RaycastHit hitInfo2 = default;
            Vector3 vector = hitInfo.collider.bounds.center - hitInfo.point;

            return Physics.Raycast(hitInfo.point,
                vector.normalized,
                out hitInfo2,
                vector.magnitude,
                targetCollisionLayerMask);
        }

        protected Vector3 GetLocalHitPointByPhysics(RaycastHit hitInfo, GameObject physicalRoot, GameObject visualRoot) =>
            MathUtil.WorldPositionToLocalPosition(PhysicsUtil.GetPulledHitPoint(hitInfo), physicalRoot);

        protected Vector3 GetLocalHitPointByVisual(RaycastHit hitInfo, GameObject physicalRoot, GameObject visualRoot) =>
            MathUtil.WorldPositionToLocalPosition(PhysicsUtil.GetPulledHitPoint(hitInfo), visualRoot);
    }
}