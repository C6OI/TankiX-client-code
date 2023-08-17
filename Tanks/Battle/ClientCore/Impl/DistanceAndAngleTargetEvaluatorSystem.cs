using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class DistanceAndAngleTargetEvaluatorSystem : ECSSystem {
        public const float MAX_PRIORITY = 1000f;

        [OnEventFire]
        public void EvaluateTargets(TargetingEvaluateEvent evt, EvaluatorNode evaluator) {
            TargetingData targetingData = evt.TargetingData;
            List<DirectionData>.Enumerator enumerator = targetingData.Directions.GetEnumerator();

            while (enumerator.MoveNext()) {
                DirectionData current = enumerator.Current;
                List<TargetData>.Enumerator enumerator2 = current.Targets.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    TargetData current2 = enumerator2.Current;

                    float num = evaluator.distanceAndAngleTargetEvaluator.DistanceWeight *
                                current2.HitDistance /
                                targetingData.FullDistance;

                    float num2 = evaluator.distanceAndAngleTargetEvaluator.AngleWeight *
                                 current.Angle /
                                 targetingData.MaxAngle;

                    current2.Priority += 1000f - (num + num2);
                }
            }
        }

        public class EvaluatorNode : Node {
            public DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator;
        }
    }
}