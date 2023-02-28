using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UserEmailShowSystem : ECSSystem {
        [OnEventFire]
        public void InitButton(NodeAddedEvent e, SingleNode<UserEmailShowButtonComponent> button, SingleNode<UserEmailUIComponent> userEmailUiComponent) {
            PlayerPrefs.SetInt("UserEmailIsVisibile", 0);
            button.component.SetEyeColor(UserEmailIsVisibile());
            userEmailUiComponent.component.EmailIsVisible = UserEmailIsVisibile();
        }

        [OnEventFire]
        public void SwitchEmailVisibility(ButtonClickEvent e, SingleNode<UserEmailShowButtonComponent> button, [JoinAll] SingleNode<UserEmailUIComponent> userEmailUiComponent) {
            PlayerPrefs.SetInt("UserEmailIsVisibile", !UserEmailIsVisibile() ? 1 : 0);
            button.component.SetEyeColor(UserEmailIsVisibile());
            userEmailUiComponent.component.EmailIsVisible = UserEmailIsVisibile();
        }

        bool UserEmailIsVisibile() => PlayerPrefs.GetInt("UserEmailIsVisibile", 1) == 1;
    }
}