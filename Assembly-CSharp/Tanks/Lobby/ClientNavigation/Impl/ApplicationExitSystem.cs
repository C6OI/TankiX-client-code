using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.main.csharp.API.Screens;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class ApplicationExitSystem : ECSSystem {
        [OnEventFire]
        public void ExitApplication(ButtonClickEvent e, SingleNode<ExitButtonComponent> node) {
            Application.Quit();
        }
    }
}