using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class TeamScoreTableSystem : ECSSystem {
        [OnEventFire]
        public void AddUserToBattleSelectTable(NodeAddedEvent e, [Combine] TeamScoreTableNode scoreTable, [Context] [JoinByBattle] [Combine] RoundUserNode roundUser,
            [JoinByTeam] TeamNode teamNode, [JoinAll] SingleNode<BattleSelectScreenComponent> screen) {
            if (scoreTable.uiTeam.TeamColor == teamNode.teamColor.TeamColor) {
                AddRow(scoreTable, roundUser);
            }
        }

        void AddRow(TeamScoreTableNode scoreTable, RoundUserNode roundUser) {
            ScoreTableRowComponent scoreTableRowComponent = scoreTable.scoreTable.AddRow();
            Entity entity = scoreTableRowComponent.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent(new ScoreTableGroupComponent(scoreTable.scoreTableGroup.Key));
            entity.AddComponent(new UserGroupComponent(roundUser.userGroup.Key));
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