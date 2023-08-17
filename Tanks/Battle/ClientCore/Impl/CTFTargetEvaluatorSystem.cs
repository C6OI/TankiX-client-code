using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class CTFTargetEvaluatorSystem : ECSSystem {
        [OnEventFire]
        public void EvaluateTargets(TargetingEvaluateEvent evt, EvaluatorNode evaluator, [JoinByUser] TankNode tankNode) {
            TargetingData targetingData = evt.TargetingData;
            List<DirectionData>.Enumerator enumerator = targetingData.Directions.GetEnumerator();

            while (enumerator.MoveNext()) {
                DirectionData current = enumerator.Current;
                List<TargetData>.Enumerator enumerator2 = current.Targets.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    TargetData current2 = enumerator2.Current;
                    GetFlagTargetBonusEvent getFlagTargetBonusEvent = new();
                    NewEvent(getFlagTargetBonusEvent).Attach(current2.Entity).Attach(evaluator).Schedule();
                    current2.Priority += getFlagTargetBonusEvent.value;
                }
            }
        }

        [OnEventFire]
        public void GetFlagTargetBouns(GetFlagTargetBonusEvent e, EvaluatorNode evaluator, TankNode tank,
            [JoinByTank] FlagNode flag) => e.value = evaluator.ctfTargetEvaluator.FlagCarrierPriorityBonus;

        public class TankNode : Node {
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class FlagNode : Node {
            public FlagComponent flag;

            public TankGroupComponent tankGroup;
        }

        public class EvaluatorNode : Node {
            public CTFTargetEvaluatorComponent ctfTargetEvaluator;
        }

        public class GetFlagTargetBonusEvent : Event {
            public float value;
        }
    }
}