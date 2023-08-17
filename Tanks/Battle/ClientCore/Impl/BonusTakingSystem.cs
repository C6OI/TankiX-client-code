using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusTakingSystem : ECSSystem {
        [OnEventFire]
        public void TakeBonus(TriggerEnterEvent e, SingleNode<BonusActiveStateComponent> bonus, SelfTankNode tank) {
            ScheduleEvent<SendTankMovementEvent>(tank);
            NewEvent<BonusTakingRequestEvent>().Attach(bonus).Attach(tank).Schedule();
        }

        [OnEventComplete]
        public void DestroyBonusBox(BonusTakenEvent e, SingleNode<BonusBoxInstanceComponent> bonus) =>
            UnityUtil.Destroy(bonus.component.BonusBoxInstance);

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;
        }

        [Not(typeof(BonusActiveStateComponent))]
        public class NotActiveBonusNode : Node {
            public BonusComponent bonus;
        }
    }
}