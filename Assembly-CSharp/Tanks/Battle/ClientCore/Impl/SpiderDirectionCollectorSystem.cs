using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class SpiderDirectionCollectorSystem : AbstractDirectionsCollectorSystem {
        [OnEventFire]
        public void CollectDirections(TargetingEvent evt, TargetingNode targeting, [JoinAll] ICollection<ActiveRemoteTank> enemyTankNodes) {
            Rigidbody rigidbody = targeting.rigidbody.Rigidbody;
            Vector3 vector = rigidbody.position + Vector3.up * 1.5f;
            TargetingData targetingData = evt.TargetingData;
            targetingData.FullDistance = targeting.unitTargetingConfig.WorkDistance;
            float num = 5f;

            foreach (ActiveRemoteTank enemyTankNode in enemyTankNodes) {
                Rigidbody rigidbody2 = enemyTankNode.rigidbody.Rigidbody;
                Vector3 forward = rigidbody.transform.forward;
                float magnitude = (rigidbody2.position - vector).magnitude;

                if (!(magnitude > targeting.unitTargetingConfig.WorkDistance)) {
                    Vector3 normalized = (rigidbody2.position - vector).normalized;
                    float angle = Mathf.Acos(Vector3.Dot(forward, normalized));
                    CollectDirection(vector, normalized, angle, targetingData);
                }
            }

            ScheduleEvent(BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), targeting);
            ScheduleEvent(BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), targeting);
        }

        public class TargetingNode : Node {
            public RigidbodyComponent rigidbody;

            public UnitReadyComponent unitReady;
            public UnitTargetingConfigComponent unitTargetingConfig;
        }

        public class ActiveRemoteTank : Node {
            public RemoteTankComponent remoteTank;

            public RigidbodyComponent rigidbody;
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;
        }
    }
}