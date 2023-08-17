using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class MineFriendAndEnemySystem : ECSSystem {
        [OnEventFire]
        public void AddEnemyComponent(NodeAddedEvent e, MineNode mine,
            [JoinByUser] SingleNode<RemoteTankComponent> remoteTank, MineNode mineA,
            [JoinByBattle] SingleNode<DMComponent> dmBattle) => mine.Entity.AddComponent<EnemyComponent>();

        [OnEventFire]
        public void AddEnemyComponent(NodeAddedEvent e, [Combine] TeamMineNode mine,
            [Context] [JoinByBattle] SelfBattleUser user) {
            if (!mine.teamGroup.Key.Equals(user.teamGroup.Key)) {
                mine.Entity.AddComponent<EnemyComponent>();
            }
        }

        [Not(typeof(EnemyComponent))]
        public class MineNode : Node {
            public BattleGroupComponent battleGroup;
            public MineComponent mine;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(EnemyComponent))]
        public class TeamMineNode : Node {
            public BattleGroupComponent battleGroup;
            public MineComponent mine;

            public TeamGroupComponent teamGroup;
        }

        public class SelfBattleUser : Node {
            public SelfBattleUserComponent selfBattleUser;

            public TeamGroupComponent teamGroup;
        }
    }
}