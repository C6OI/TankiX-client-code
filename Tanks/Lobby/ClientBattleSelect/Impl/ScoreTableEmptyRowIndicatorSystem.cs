using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableEmptyRowIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void Localize(NodeAddedEvent e, SingleNode<ScoreTableEmptyRowIndicatorComponent> indicator,
            SingleNode<ScoreTableEmptyRowTextComponent> text) => indicator.component.Text = text.component.Text;
    }
}