using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class HelpButtonSystem : ECSSystem {
        [OnEventFire]
        public void NavigateToHelp(ButtonClickEvent e, SingleNode<HelpButtonComponent> button) {
            Application.OpenURL(button.component.Url);
        }
    }
}