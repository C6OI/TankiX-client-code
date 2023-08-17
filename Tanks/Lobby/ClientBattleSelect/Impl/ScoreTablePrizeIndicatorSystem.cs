using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTablePrizeIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void ShowUserPrizeWhenAwarded(GivePrizeUserEvent e, SingleNode<RoundUserComponent> roundUser,
            [JoinByUser] PrizeNode prize) => prize.scoreTablePrizeIndicator.Prize = e.Prize;

        public class PrizeNode : Node {
            public ScoreTablePrizeIndicatorComponent scoreTablePrizeIndicator;
        }
    }
}