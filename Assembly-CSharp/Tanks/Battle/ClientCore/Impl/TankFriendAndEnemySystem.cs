using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankFriendAndEnemySystem : ECSSystem {
        [OnEventFire]
        public void SetSelfTankAsDefined(NodeAddedEvent e, SingleNode<SelfTankComponent> tank) {
            tank.Entity.AddComponent<TankFriendlyEnemyStatusDefinedComponent>();
        }

        [OnEventFire]
        public void AddEnemyComponent(NodeAddedEvent e, [Combine] RemoteTankNode tank, [JoinByBattle] SingleNode<DMComponent> dmBattle) {
            tank.Entity.AddComponent<EnemyComponent>();
            tank.Entity.AddComponent<TankFriendlyEnemyStatusDefinedComponent>();
        }

        [OnEventFire]
        public void AddEnemyComponent(NodeAddedEvent e, [Combine] TeamTankNode tank, [Context] [JoinByBattle] SelfBattleUserAsTank userAsTank) {
            if (!tank.teamGroup.Key.Equals(userAsTank.teamGroup.Key)) {
                tank.Entity.AddComponent<EnemyComponent>();
            }

            tank.Entity.AddComponent<TankFriendlyEnemyStatusDefinedComponent>();
        }

        [OnEventFire]
        public void DefineForSpec(NodeAddedEvent e, [Combine] RemoteTankNode tank, [Context] [JoinByBattle] SelfBattleUserAsSpectator userAsSpectator) {
            tank.Entity.AddComponent<TankFriendlyEnemyStatusDefinedComponent>();
        }

        [Not(typeof(EnemyComponent))]
        public class RemoteTankNode : Node {
            public BattleGroupComponent battleGroup;
            public RemoteTankComponent remoteTank;
        }

        public class TeamTankNode : RemoteTankNode {
            public TeamGroupComponent teamGroup;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class SelfBattleUserAsTank : SelfBattleUserNode {
            public TeamGroupComponent teamGroup;
            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class SelfBattleUserAsSpectator : SelfBattleUserNode {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }
    }
}