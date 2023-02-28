using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class TeamBattleScoreTableSystem : ECSSystem {
        [OnEventFire]
        public void InitRowsCache(NodeAddedEvent e, [Combine] TeamScoreTableNode scoreTable, [Context] [JoinByBattle] BattleNode battle,
            [JoinAll] SingleNode<BattleScreenComponent> screen) {
            scoreTable.scoreTable.InitRowsCache(battle.userLimit.TeamLimit, scoreTable.scoreTableUserRowIndicators.indicators);
            scoreTable.Entity.AddComponent<ScoreTableRowsCacheInitedComponent>();
        }

        [OnEventFire]
        public void AddUserToBattleTable(NodeAddedEvent e, [Combine] InitedScoreTable scoreTable, [Context] [JoinByBattle] [Combine] RoundUserNode roundUser,
            [JoinByTeam] TeamUIColorNode teamNode, [JoinAll] SingleNode<BattleScreenComponent> screen) {
            if (scoreTable.uiTeam.TeamColor == teamNode.colorInBattle.TeamColor) {
                AddRow(scoreTable, roundUser);
            }
        }

        void AddRow(InitedScoreTable scoreTable, RoundUserNode roundUser) {
            ScoreTableRowComponent scoreTableRowComponent = scoreTable.scoreTable.AddRow();
            Entity entity = scoreTableRowComponent.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent(new ScoreTableGroupComponent(scoreTable.scoreTableGroup.Key));
            entity.AddComponent(new UserGroupComponent(roundUser.userGroup.Key));

            foreach (ScoreTableRowIndicator value in scoreTableRowComponent.indicators.Values) {
                EntityBehaviour component = value.GetComponent<EntityBehaviour>();

                if (!(component == null)) {
                    component.BuildEntity(entity);
                }
            }
        }

        public class TeamScoreTableNode : Node {
            public BattleGroupComponent battleGroup;
            public ScoreTableComponent scoreTable;

            public ScoreTableGroupComponent scoreTableGroup;

            public ScoreTableUserRowIndicatorsComponent scoreTableUserRowIndicators;

            public UITeamComponent uiTeam;
        }

        public class InitedScoreTable : TeamScoreTableNode {
            public ScoreTableRowsCacheInitedComponent scoreTableRowsCacheInited;
        }

        public class RoundUserNode : Node {
            public BattleGroupComponent battleGroup;
            public RoundUserComponent roundUser;

            public TeamGroupComponent teamGroup;

            public UserGroupComponent userGroup;
        }

        public class TeamUIColorNode : Node {
            public ColorInBattleComponent colorInBattle;
            public TeamGroupComponent teamGroup;
        }

        public class BattleNode : Node {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;

            public UserLimitComponent userLimit;
        }
    }
}