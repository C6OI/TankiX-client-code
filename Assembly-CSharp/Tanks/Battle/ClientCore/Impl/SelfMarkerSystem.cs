using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class SelfMarkerSystem : ECSSystem {
        [OnEventFire]
        public void MarkTank(NodeAddedEvent e, TankNode tank, [Context] [JoinByUser] UserNode user) {
            if (user.Entity.HasComponent<SelfComponent>()) {
                tank.Entity.AddComponent<SelfTankComponent>();
            } else {
                tank.Entity.AddComponent<RemoteTankComponent>();
            }
        }

        [OnEventFire]
        public void MarkSelfBattleUser(NodeAddedEvent e, BattleUserNode battleUser, [Context] [JoinByUser] SelfUserNode user) {
            battleUser.Entity.AddComponent<SelfBattleUserComponent>();
        }

        [OnEventFire]
        public void MarkBattle(NodeAddedEvent e, SelfBattleUserNode battleUser, [JoinByBattle] BattleNode battle) {
            battle.Entity.AddComponent<SelfComponent>();
        }

        [OnEventFire]
        public void UnmarkBattle(NodeRemoveEvent e, SelfBattleUserNode battleUser, [JoinByBattle] BattleNode battle) {
            battle.Entity.RemoveComponent<SelfComponent>();
        }

        public class UserNode : Node {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node {
            public SelfComponent self;

            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class TankNode : Node {
            public TankComponent tank;
            public UserGroupComponent userGroup;
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleUserComponent battleUser;

            public UserGroupComponent userGroup;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleUserComponent battleUser;

            public SelfComponent self;

            public UserGroupComponent userGroup;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;
        }
    }
}