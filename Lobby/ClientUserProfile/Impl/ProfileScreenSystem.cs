using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Lobby.ClientUserProfile.Impl.ChangeUID;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientUserProfile.Impl {
    public class ProfileScreenSystem : ECSSystem {
        [OnEventFire]
        public void ShowScreenElementsForSelfUser(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen,
            [JoinByUser] SelfUserNode selfUser) {
            profileScreen.profileScreen.SelfUserExperienceIndicator.SetActive(true);
            profileScreen.profileScreen.LogoutButton.SetActive(true);
            profileScreen.profileScreen.GoToConfirmEmailScreenButton.SetActive(true);
            profileScreen.profileScreen.GoToChangeUidScreenButton.SetActive(true);
        }

        [OnEventFire]
        public void ShowScreenElementsForRemoteUser(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen,
            [JoinByUser] RemoteUserNode remoteUser) =>
            profileScreen.profileScreen.RemoteUserExperienceIndicator.SetActive(true);

        [OnEventFire]
        public void SelectEmailSettingsScreenToShow(ButtonClickEvent e,
            SingleNode<GoToEmailSettingsScreenButtonComponent> button, [JoinAll] SelfUserNode selfUser) {
            if (selfUser.Entity.HasComponent<UnconfirmedUserEmailComponent>() &&
                !string.IsNullOrEmpty(selfUser.Entity.GetComponent<UnconfirmedUserEmailComponent>().Email)) {
                ScheduleEvent<ShowScreenLeftEvent<ConfirmUserEmailScreenComponent>>(button);
            } else if (selfUser.Entity.HasComponent<ConfirmedUserEmailComponent>()) {
                ScheduleEvent<ShowScreenLeftEvent<ViewUserEmailScreenComponent>>(button);
            } else {
                ScheduleEvent<ShowScreenLeftEvent<ChangeUserEmailScreenComponent>>(button);
            }
        }

        [OnEventFire]
        public void SelectChangeUIDScreenToShow(ButtonClickEvent e, SingleNode<GoToChangeUIDScreenBiuttonComponent> button,
            [JoinAll] SelfUserNode selfUser) => ScheduleEvent<ShowScreenLeftEvent<ChangeUIDScreenComponent>>(button);

        public class ProfileScreenWithUserGroupNode : Node {
            public ActiveScreenComponent activeScreen;
            public ProfileScreenComponent profileScreen;

            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(SelfUserComponent))]
        public class RemoteUserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }
    }
}