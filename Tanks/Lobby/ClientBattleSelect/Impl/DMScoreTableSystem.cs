using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class DMScoreTableSystem : ECSSystem {
        [OnEventFire]
        public void AddUser(NodeAddedEvent e, [Combine] DMScoreTableNode scoreTable,
            [Combine] [JoinByBattle] [Context] RoundUserNode roundUser) {
            ScoreTableRowComponent scoreTableRowComponent = scoreTable.scoreTable.AddRow();
            Entity entity = scoreTableRowComponent.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent(new ScoreTableGroupComponent(scoreTable.scoreTableGroup.Key));
            entity.AddComponent(new UserGroupComponent(roundUser.userGroup.Key));
        }

        public class RoundUserNode : Node {
            public BattleGroupComponent battleGroup;
            public RoundUserStatisticsComponent roundUserStatistics;

            public UserGroupComponent userGroup;
        }

        public class DMScoreTableNode : Node {
            public BattleGroupComponent battleGroup;

            public DMScoreTableComponent dmScoreTable;
            public ScoreTableComponent scoreTable;

            public ScoreTableGroupComponent scoreTableGroup;

            public ScoreTableUserRowIndicatorsComponent scoreTableUserRowIndicators;
        }
    }
}