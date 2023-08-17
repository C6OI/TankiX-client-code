using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class TeamScoreTableSystem : ECSSystem {
        [OnEventFire]
        public void AddUser(NodeAddedEvent e, [Combine] TeamScoreTableNode scoreTable,
            [Combine] [JoinByBattle] [Context] RoundUserNode roundUser, [JoinByTeam] TeamNode teamNode) {
            if (scoreTable.uiTeam.TeamColor == teamNode.teamColor.TeamColor) {
                ScoreTableRowComponent scoreTableRowComponent = scoreTable.scoreTable.AddRow();
                Entity entity = scoreTableRowComponent.GetComponent<EntityBehaviour>().Entity;
                entity.AddComponent(new ScoreTableGroupComponent(scoreTable.scoreTableGroup.Key));
                entity.AddComponent(new UserGroupComponent(roundUser.userGroup.Key));
            }
        }

        public class TeamScoreTableNode : Node {
            public BattleGroupComponent battleGroup;
            public ScoreTableComponent scoreTable;

            public ScoreTableGroupComponent scoreTableGroup;

            public ScoreTableUserRowIndicatorsComponent scoreTableUserRowIndicators;

            public UITeamComponent uiTeam;
        }

        public class RoundUserNode : Node {
            public BattleGroupComponent battleGroup;
            public RoundUserComponent roundUser;

            public TeamGroupComponent teamGroup;

            public UserGroupComponent userGroup;
        }

        public class TeamNode : Node {
            public TeamColorComponent teamColor;
            public TeamGroupComponent teamGroup;
        }
    }
}