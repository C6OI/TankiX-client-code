using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Lobby.ClientUserProfile.Impl {
    public class LogoutSystem : ECSSystem {
        [OnEventFire]
        public void Logout(ConfirmButtonClickYesEvent e, SingleNode<LogoutButtonComponent> node) {
            PlayerPrefs.DeleteKey("TOToken");
            PlayerPrefs.SetInt("RemeberMeFlag", 0);
            ScheduleEvent<SwitchToEntranceSceneEvent>(node);
        }
    }
}