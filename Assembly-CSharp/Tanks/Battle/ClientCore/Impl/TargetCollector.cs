using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TargetCollector {
        public static int SAFE_ITERATION_LIMIT = 20;

        readonly Entity ownerEntity;

        public TargetCollector(Entity ownerEntity) => this.ownerEntity = ownerEntity;

        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        public void Collect(TargetValidator validator, TargetingData targetingData, int layerMask = 0) {
            foreach (DirectionData direction in targetingData.Directions) {
                Collect(validator, targetingData.FullDistance, direction, layerMask);
            }
        }

        public DirectionData Collect(TargetValidator validator, float fullDistance, Vector3 origin, Vector3 dir, int layerMask = 0) {
            DirectionData directionData = BattleCache.directionData.GetInstance().Init(origin, dir, 0f);
            Collect(validator, fullDistance, directionData, layerMask);
            return directionData;
        }

        public void Collect(TargetValidator validator, float fullDistance, DirectionData direction, int layerMask = 0) {
            Ray ray = default;
            ray.origin = direction.Origin;
            ray.direction = direction.Dir;
            Ray ray2 = ray;
            float num = 0f;
            validator.Begin();
            Entity obj = null;
            HashSet<Entity> hashSet = new();
            int num2 = SAFE_ITERATION_LIMIT;
            int layerMask2 = layerMask == 0 ? validator.LayerMask : layerMask;
            RaycastHit hitInfo;

            while (num2 > 0 && Physics.Raycast(ray2, out hitInfo, fullDistance - num, layerMask2)) {
                num2--;
                float distance = hitInfo.distance;
                num += distance;
                Rigidbody rigidbody = hitInfo.rigidbody;
                TargetBehaviour targetBehaviour = !rigidbody ? null : rigidbody.GetComponentInParent<TargetBehaviour>();
                Vector3 normal = hitInfo.normal;

                if (!rigidbody || !(targetBehaviour != null) || targetBehaviour.TargetEntity == null) {
                    direction.StaticHit = new StaticHit {
                        Position = PhysicsUtil.GetPulledHitPoint(hitInfo),
                        Normal = hitInfo.normal
                    };

                    if (validator.BreakOnStaticHit()) {
                        break;
                    }

                    ray2 = validator.ContinueOnStaticHit(ray2, normal, distance);
                    continue;
                }

                Entity targetEntity = targetBehaviour.TargetEntity;

                if (hashSet.Contains(targetEntity) || targetEntity.Equals(obj)) {
                    ray2 = validator.Continue(ray2, hitInfo.distance);
                    continue;
                }

                obj = targetEntity;

                if (validator.CanSkip(targetEntity) || targetBehaviour.CanSkip(ownerEntity)) {
                    ray2 = validator.Continue(ray2, hitInfo.distance);
                } else if (validator.AcceptAsTarget(targetEntity) && targetBehaviour.AcceptAsTarget(ownerEntity)) {
                    TargetData targetData = BattleCache.targetData.GetInstance().Init(targetBehaviour.TargetEntity, targetBehaviour.TargetIcarnationEntity);
                    validator.FillTargetData(targetData, hitInfo, targetBehaviour.gameObject, ray2, num);
                    direction.Targets.Add(targetData);
                    hashSet.Add(targetEntity);

                    if (validator.BreakOnTargetHit(targetEntity)) {
                        break;
                    }

                    ray2 = validator.ContinueOnTargetHit(ray2, normal, distance);
                } else {
                    direction.StaticHit = new StaticHit {
                        Position = PhysicsUtil.GetPulledHitPoint(hitInfo),
                        Normal = hitInfo.normal
                    };

                    if (validator.BreakOnStaticHit()) {
                        break;
                    }

                    ray2 = validator.ContinueOnStaticHit(ray2, normal, distance);
                }
            }
        }
    }
}