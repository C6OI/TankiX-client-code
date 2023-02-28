using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ShowIndicatorOnRoundRestartSystem : ECSSystem {
        [OnEventFire]
        public void Show(NodeAddedEvent e, [Combine] SingleNode<RoundRestartingStateComponent> restartingRound,
            [Context] [Combine] SingleNode<ShowIndicatorOnRoundRestartComponent> indicator) {
            indicator.component.Show();
        }

        [OnEventFire]
        public void Hide(NodeAddedEvent e, [Combine] SingleNode<RoundActiveStateComponent> restartingRound,
            [Context] [Combine] SingleNode<ShowIndicatorOnRoundRestartComponent> indicator) {
            indicator.component.Hide();
        }
    }
}