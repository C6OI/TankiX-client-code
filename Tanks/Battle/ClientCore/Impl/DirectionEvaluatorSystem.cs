using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class DirectionEvaluatorSystem : ECSSystem {
        [OnEventComplete]
        public void EvaluateDirections(TargetingEvaluateEvent evt, EvaluatorNode evaluator) {
            List<DirectionData> directions = evt.TargetingData.Directions;
            float num = float.MinValue;
            TargetingData targetingData = evt.TargetingData;

            float num2 = !evaluator.Entity.HasComponent<DamageWeakeningByTargetComponent>() ? 1f
                             : ((EntityInternal)evaluator.Entity).GetComponent<DamageWeakeningByTargetComponent>()
                               .DamagePercent /
                               100f;

            DirectionData directionData = directions.First();
            int bestDirectionIndex = 0;

            for (int i = 0; i < targetingData.Directions.Count; i++) {
                DirectionData directionData2 = targetingData.Directions[i];
                bool flag = false;
                List<TargetData>.Enumerator enumerator = directionData2.Targets.GetEnumerator();

                while (enumerator.MoveNext()) {
                    TargetData current = enumerator.Current;

                    if (current.ValidTarget) {
                        flag = true;
                        directionData2.Priority += current.Priority * (float)Math.Pow(num2, current.PriorityWeakeningCount);
                    }
                }

                if (flag && directionData2.Priority > num) {
                    directionData = directionData2;
                    bestDirectionIndex = i;
                    num = directionData.Priority;
                }
            }

            evt.TargetingData.BestDirection = directionData;
            evt.TargetingData.BestDirectionIndex = bestDirectionIndex;
        }

        public class EvaluatorNode : Node {
            public DirectionEvaluatorComponent directionEvaluator;
        }
    }
}