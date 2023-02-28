using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class FlagCollisionSystem : ECSSystem {
        [OnEventFire]
        public void SendCollisionEvent(TankFlagCollisionEvent e, TankNode tank, [JoinByBattle] SingleNode<RoundActiveStateComponent> round, [Context] [JoinByBattle] FlagNode flag) {
            ScheduleEvent<SendTankMovementEvent>(tank);
            NewEvent<FlagCollisionRequestEvent>().Attach(tank).Attach(flag).Schedule();
        }

        public class TankNode : Node {
            public BattleGroupComponent battleGroup;
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankSyncComponent tankSync;

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