using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LogoutSystem : ECSSystem {
        [OnEventFire]
        public void ShowLogoutConfirmDialog(ButtonClickEvent e, SingleNode<LogoutButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            LogoutConfirmWindowComponent logoutConfirmWindowComponent = dialogs.component.Get<LogoutConfirmWindowComponent>();
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            logoutConfirmWindowComponent.Show(animators);
        }

        [OnEventFire]
        public void Logout(DialogConfirmEvent e, LogoutWindowNode node) {
            PlayerPrefs.DeleteKey("TOToken");
            PlayerPrefs.SetInt("SteamAuthentication", 1);
            PlayerPrefs.SetInt("RemeberMeFlag", 0);
            ScheduleEvent<SwitchToEntranceSceneEvent>(node);
        }

        public class LogoutWindowNode : Node {
            public LogoutConfirmWindowComponent logoutConfirmWindow;
        }

        public class ProfileAccountSectionNode : Node {
            public ProfileAccountSectionUIComponent profileAccountSectionUI;
        }
    }
}