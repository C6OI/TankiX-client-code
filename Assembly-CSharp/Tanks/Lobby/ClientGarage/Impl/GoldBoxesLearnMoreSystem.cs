using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoldBoxesLearnMoreSystem : ECSSystem {
        [OnEventFire]
        public void ShowInfoDialog(ButtonClickEvent e, SingleNode<GoldBoxesLearnMoreButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs60) {
            dialogs60.component.Get<GoldBoxesLearnMoreComponent>().Show();
        }
    }
}