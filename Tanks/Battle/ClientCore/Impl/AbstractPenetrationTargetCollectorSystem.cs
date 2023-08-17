using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public abstract class AbstractPenetrationTargetCollectorSystem : AbstractTargetCollectorSystem {
        [Inject] public new static BattleFlowInstancesCache BattleCache { get; set; }

        protected void CollectTargetsOnDirectionsByTargetingColliders(TargetingData targetingData,
            TankCollidersComponent tankColliders) {
            RaycastExclude raycastExclude = new(tankColliders.TargetingColliders);

            try {
                CollectTargetsOnDirections(targetingData,
                    LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS,
                    LayerMasks.STATIC,
                    AddTargetingCollidersToExclusion,
                    GetLocalHitPointByPhysics);
            } finally {
                raycastExclude.Dispose();
            }
        }

        protected void CollectTargetsOnDirectionsByMeshColliders(TargetingData targetingData,
            TankCollidersComponent tankColliders) {
            RaycastExclude raycastExclude = new(tankColliders.VisualTriggerColliders);

            try {
                CollectTargetsOnDirections(targetingData,
                    LayerMasks.VISUAL_TARGETING,
                    LayerMasks.VISUAL_STATIC,
                    AddMeshCollidersToExclusion,
                    GetLocalHitPointByVisual);
            } finally {
                raycastExclude.Dispose();
            }
        }

        void CollectTargetsOnDirections(TargetingData targetingData, int targetLayerMask, int targetCollisionLayerMask,
            Action<List<GameObject>, Entity> addExclusionObjects,
            Func<RaycastHit, GameObject, GameObject, Vector3> CalculateLocalHitPoint) {
            List<GameObject> list = new();

            foreach (DirectionData direction in targetingData.Directions) {
                Vector3 origin = direction.Origin;
                Vector3 dir = direction.Dir;
                float num = 0f;
                int num2 = 0;
                RaycastHit hitInfo;

                while (Raycast(origin, dir, out hitInfo, targetingData.FullDistance - num, list, targetLayerMask)) {
                    num += hitInfo.distance;
                    GameObject gameObject = hitInfo.transform.gameObject;
                    TargetBehaviour componentInParent = gameObject.GetComponentInParent<TargetBehaviour>();

                    if (componentInParent == null) {
                        direction.StaticHit = new StaticHit {
                            Position = PhysicsUtil.GetPulledHitPoint(hitInfo),
                            Normal = hitInfo.normal
                        };

                        break;
                    }

                    Entity entity = componentInParent.Entity;

                    if (!IsValidTarget(entity)) {
                        direction.StaticHit = new StaticHit {
                            Position = hitInfo.point,
                            Normal = hitInfo.normal
                        };
                    } else {
                        GameObject gameObject2 = componentInParent.gameObject.GetComponentInChildren<PhysicalRootBehaviour>()
                            .gameObject;

                        GameObject gameObject3 = componentInParent.gameObject
                            .GetComponentInChildren<TankVisualRootComponent>().gameObject;

                        if (!CheckTargetCollision(hitInfo, targetCollisionLayerMask)) {
                            TargetData targetData = BattleCache.targetData.GetInstance().Init(entity);
                            targetData.HitPoint = hitInfo.point;
                            targetData.LocalHitPoint = CalculateLocalHitPoint(hitInfo, gameObject2, gameObject3);
                            targetData.TargetPosition = gameObject2.transform.position;
                            targetData.HitDistance = num;
                            targetData.HitDirection = dir;
                            targetData.PriorityWeakeningCount = num2;
                            direction.Targets.Add(targetData);
                        }
                    }

                    addExclusionObjects(list, entity);
                    origin = hitInfo.point;
                    num2++;
                }

                list.Clear();
            }
        }

        void AddTargetingCollidersToExclusion(List<GameObject> excludedGameObjects, Entity tank) =>
            excludedGameObjects.AddRange(((EntityInternal)tank).GetComponent<TankCollidersComponent>().TargetingColliders);

        void AddMeshCollidersToExclusion(List<GameObject> excludedGameObjects, Entity tank) =>
            excludedGameObjects.AddRange(
                ((EntityInternal)tank).GetComponent<TankCollidersComponent>().VisualTriggerColliders);

        bool IsValidTarget(Entity targetEntity) => targetEntity.HasComponent<TankActiveStateComponent>();

        bool Raycast(Vector3 origin, Vector3 dir, out RaycastHit hitInfo, float distance, List<GameObject> excludeObjects,
            int mask) {
            RaycastExclude raycastExclude = new(excludeObjects);

            try {
                return Physics.Raycast(origin, dir, out hitInfo, distance, mask);
            } finally {
                raycastExclude.Dispose();
            }
        }
    }
}