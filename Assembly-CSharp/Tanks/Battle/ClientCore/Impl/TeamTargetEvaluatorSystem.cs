using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TeamTargetEvaluatorSystem : ECSSystem {
        [OnEventFire]
        public void EvaluateTargets(TargetingEvaluateEvent evt, EvaluatorNode evaluator, [JoinByUser] TankNode tankNode, [JoinByTeam] TeamNode team) {
            TargetingData targetingData = evt.TargetingData;
            long key = team.teamGroup.Key;
            List<DirectionData>.Enumerator enumerator = targetingData.Directions.GetEnumerator();

            while (enumerator.MoveNext()) {
                DirectionData current = enumerator.Current;
                List<TargetData>.Enumerator enumerator2 = current.Targets.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    TargetData current2 = enumerator2.Current;

                    if (current2.TargetEntity.GetComponent<TeamGroupComponent>().Key == key) {
                        current2.ValidTarget = false;
                    }
                }
            }
        }

        public class TankNode : Node {
            public TankComponent tank;

            public TeamGroupComponent teamGroup;
        }

        public class EvaluatorNode : Node {
            public TeamTargetEvaluatorComponent teamTargetEvaluator;
        }

        public class TeamNode : Node {
            public TeamComponent team;

            public TeamGroupComponent teamGroup;
        }
    }
}