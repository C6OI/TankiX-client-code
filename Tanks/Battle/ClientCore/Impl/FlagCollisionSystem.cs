using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class FlagCollisionSystem : ECSSystem {
        [OnEventFire]
        public void SendCollisionEvent(TankFlagCollisionEvent e, TankNode tank,
            [JoinByBattle] SingleNode<RoundActiveStateComponent> round, [JoinByBattle] [Context] FlagNode flag) {
            ScheduleEvent<SendTankMovementEvent>(tank);
            NewEvent<FlagCollisionRequestEvent>().Attach(tank).Attach(flag).Schedule();
        }

        public class TankNode : Node {
            public BattleGroupComponent battleGroup;

            public SelfTankComponent selfTank;
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TeamGroupComponent teamGroup;
        }

        public class FlagNode : Node {
            public BattleGroupComponent battleGroup;
            public FlagComponent flag;

            public FlagColliderComponent flagCollider;

            public FlagInstanceComponent flagInstance;

            public TeamGroupComponent teamGroup;
        }
    }
}