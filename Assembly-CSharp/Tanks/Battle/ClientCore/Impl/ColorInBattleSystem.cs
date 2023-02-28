using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ColorInBattleSystem : ECSSystem {
        [OnEventFire]
        public void AddTeamUIColor(NodeAddedEvent e, SelfTankNode selfTank, [Combine] [JoinByBattle] TeamNode teamNode) {
            TeamColor color = !teamNode.teamGroup.Key.Equals(selfTank.teamGroup.Key) ? TeamColor.RED : TeamColor.BLUE;
            teamNode.Entity.AddComponent(new ColorInBattleComponent(color));
        }

        [OnEventFire]
        public void AddSpectatorUIColor(NodeAddedEvent e, SpectatorBattleUserNode spectatorBattleUser, [Combine] [JoinByBattle] TeamNode teamNode) {
            teamNode.Entity.AddComponent(new ColorInBattleComponent(teamNode.teamColor.TeamColor));
        }

        [OnEventFire]
        public void AddDMSpectatorUIColor(NodeAddedEvent e, [Combine] BattleUserNode battleUser, [JoinByBattle] DMNode battle,
            [Context] [JoinByBattle] SpectatorBattleUserNode spectatorBattleUser) {
            if (!spectatorBattleUser.Entity.Id.Equals(battleUser.Entity.Id)) {
                battleUser.Entity.AddComponent(new ColorInBattleComponent(TeamColor.NONE));
            }
        }

        [OnEventFire]
        public void AddDMUIColor(NodeAddedEvent e, [Combine] BattleUserNode battleUser, [JoinByBattle] DMNode battle, [Context] [JoinByBattle] TankBattleUserNode tankBattleUser) {
            if (!tankBattleUser.Entity.Id.Equals(battleUser.Entity.Id)) {
                battleUser.Entity.AddComponent(new ColorInBattleComponent(TeamColor.RED));
            }
        }

        [OnEventFire]
        public void AddDMUIColor(NodeAddedEvent e, TankBattleUserNode tankBattleUser, [JoinByBattle] DMNode battle) {
            tankBattleUser.Entity.AddComponent(new ColorInBattleComponent(TeamColor.BLUE));
        }

        public class TeamNode : Node {
            public BattleGroupComponent battleGroup;
            public TeamComponent team;

            public TeamColorComponent teamColor;

            public TeamGroupComponent teamGroup;
        }

        public class SelfTankNode : Node {
            public BattleGroupComponent battleGroup;

            public SelfTankComponent selfTank;

            public TeamGroupComponent teamGroup;
        }

        public class SpectatorBattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public SelfBattleUserComponent selfBattleUser;

            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class TankBattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public SelfBattleUserComponent selfBattleUser;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleUserComponent battleUser;

            public UserGroupComponent userGroup;
        }

        public class DMNode : Node {
            public BattleGroupComponent battleGroup;

            public DMComponent dm;
        }
    }
}