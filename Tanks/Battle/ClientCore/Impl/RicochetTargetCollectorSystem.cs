using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class RicochetTargetCollectorSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventFire]
        public void CollectTargetsOnDirections(CollectTargetsEvent evt, TargetCollectorNode targetCollectorNode,
            [JoinByTank] SingleNode<TankCollidersComponent> tankCollidersNode) {
            TargetingData targetingData = evt.TargetingData;
            RaycastExclude raycastExclude = new(tankCollidersNode.component.TargetingColliders);

            try {
                foreach (DirectionData direction in targetingData.Directions) {
                    FindTargetOnDirection(targetingData, direction);
                }
            } finally {
                raycastExclude.Dispose();
            }
        }

        void FindTargetOnDirection(TargetingData targetingData, DirectionData direction) {
            float num = targetingData.FullDistance;
            Vector3 origin = direction.Origin;
            Vector3 direction2 = direction.Dir;
            int num2 = 0;
            RaycastHit hitInfo;
            TargetBehaviour componentInParent;
            bool flag;

            do {
                if (!Physics.Raycast(origin, direction2, out hitInfo, num, LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS)) {
                    Debug.DrawRay(origin, direction2 * hitInfo.distance, Color.grey, 1f);
                    return;
                }

                num = Math.Max(0f, num - hitInfo.distance);
                GameObject gameObject = hitInfo.transform.gameObject;
                componentInParent = gameObject.GetComponentInParent<TargetBehaviour>();
                flag = !IsValidTarget(componentInParent);

                if (flag) {
                    Debug.DrawRay(origin, direction2 * hitInfo.distance, Color.green, 1f);

                    direction.StaticHit = new StaticHit {
                        Position = hitInfo.point,
                        Normal = hitInfo.normal
                    };

                    num2++;
                    CalculateRicochet(hitInfo.point, hitInfo.normal, ref origin, ref direction2);
                }
            } while (flag && num > 0f);

            if (!flag) {
                Debug.DrawRay(origin, direction2 * hitInfo.distance, Color.red, 1f);
                Entity entity = componentInParent.Entity;
                GameObject gameObject2 = componentInParent.gameObject;
                GameObject gameObject3 = gameObject2.GetComponentInChildren<PhysicalRootBehaviour>().gameObject;
                TargetData targetData = BattleCache.targetData.GetInstance().Init(entity);
                targetData.HitPoint = hitInfo.point;
                targetData.LocalHitPoint = MathUtil.WorldPositionToLocalPosition(hitInfo.point, gameObject3);
                targetData.TargetPosition = gameObject3.transform.position;
                targetData.HitDistance = targetingData.FullDistance - num;
                targetData.PriorityWeakeningCount = num2;
                direction.Targets.Add(targetData);
            }
        }

        [OnEventFire]
        public void EvaluateTargets(TargetingEvaluateEvent e, TargetCollectorNode weapon) {
            List<DirectionData>.Enumerator enumerator = e.TargetingData.Directions.GetEnumerator();

            while (enumerator.MoveNext()) {
                DirectionData current = enumerator.Current;
                List<TargetData>.Enumerator enumerator2 = current.Targets.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    TargetData current2 = enumerator2.Current;

                    if (current2.PriorityWeakeningCount == 0) {
                        current2.Priority += 2f;
                    }
                }
            }
        }

        static void CalculateRicochet(Vector3 hitPosition, Vector3 hitNormal, ref Vector3 origin, ref Vector3 direction) {
            float num = 0.5f;
            origin = hitPosition - direction * num;
            direction = (direction - 2f * Vector3.Dot(direction, hitNormal) * hitNormal).normalized;
        }

        bool IsValidTarget(TargetBehaviour targetBehaviour) =>
            targetBehaviour != null && targetBehaviour.Entity.HasComponent<TankActiveStateComponent>();

        public class TargetCollectorNode : Node {
            public RicochetTargetCollectorComponent ricochetTargetCollector;
        }
    }
}