using Lobby.ClientNavigation.main.csharp.API.Screens;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientNavigation.Impl {
    public class ApplicationExitSystem : ECSSystem {
        [OnEventFire]
        public void ExitApplication(ButtonClickEvent e, SingleNode<ExitButtonComponent> node) => Application.Quit();
    }
}