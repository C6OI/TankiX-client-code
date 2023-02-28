using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultStatScreenSystem : ECSSystem {
        [OnEventFire]
        public void ShowMatchDetails(NodeAddedEvent e, SingleNode<BattleResultsScreenStatComponent> screen, [JoinAll] ResultsNode results) {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            Entity entityById = GetEntityById(resultForClient.MapId);
            string name = entityById.GetComponent<DescriptionItemComponent>().Name;
            BattleMode battleMode = resultForClient.BattleMode;
            string battleDescription = string.Format("{0}, {1}", battleMode, name);
            screen.component.BattleDescription = battleDescription;

            if (results.battleResults.ResultForClient.BattleMode == BattleMode.DM) {
                screen.component.ShowDMMatchDetails();
            } else {
                screen.component.ShowTeamMatchDetails();
            }
        }

        public class ResultsNode : Node {
            public BattleResultsComponent battleResults;
        }
    }
}