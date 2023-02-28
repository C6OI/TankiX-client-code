using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class CTFTargetEvaluatorSystem : ECSSystem {
        [OnEventFire]
        public void EvaluateTargets(TargetingEvaluateEvent evt, EvaluatorNode evaluator, [JoinByUser] TankNode tankNode) {
            foreach (DirectionData direction in evt.TargetingData.Directions) {
                foreach (TargetData target in direction.Targets) {
                    NewEvent(GetFlagTargetBonusEvent.INSTANCE).Attach(target.TargetEntity).Attach(evaluator).Schedule();
                    target.Priority += GetFlagTargetBonusEvent.INSTANCE.Value;
                }
            }
        }

        [OnEventFire]
        public void GetFlagTargetBonus(GetFlagTargetBonusEvent e, EvaluatorNode evaluator, TankNode tank, [Combine] [JoinByTank] FlagNode flag) {
            e.Value = evaluator.ctfTargetEvaluator.FlagCarrierPriorityBonus;
        }

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
            public static GetFlagTargetBonusEvent INSTANCE = new();

            GetFlagTargetBonusEvent() => Value = 0f;

            public float Value { get; set; }
        }
    }
}