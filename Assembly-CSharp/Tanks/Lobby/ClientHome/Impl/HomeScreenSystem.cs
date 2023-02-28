using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientHome.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNavigation.Impl;
using Tanks.Lobby.ClientSettings.API;

namespace Tanks.Lobby.ClientHome.Impl {
    public class HomeScreenSystem : ECSSystem {
        [OnEventFire]
        public void ShowHomeScreen(NodeRemoveEvent e, SingleNode<PreloadAllResourcesComponent> preloadAllResourcesRequest, [JoinAll] TopPanelNode topPanel) {
            ScheduleEvent<ShowScreenNoAnimationEvent<MainScreenComponent>>(topPanel);
        }

        [OnEventFire]
        public void ShowSettingsScreen(ButtonClickEvent e, SingleNode<SettingsButtonComponent> node) {
            ScheduleEvent<ShowScreenLeftEvent<SettingsScreenComponent>>(node);
            MainScreenComponent.Instance.SendShowScreenStat(LogScreen.Settings);
        }

        [OnEventFire]
        public void GroupWithUser(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen, SelfUserNode selfUser) {
            ScheduleEvent(new SetScreenHeaderEvent {
                Animate = false,
                Header = string.Empty
            }, homeScreen);
        }

        [OnEventFire]
        public void FinalizeCompactWindow(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen) {
            GraphicsSettings.INSTANCE.DisableCompactScreen();
        }

        public class TopPanelNode : Node {
            public TopPanelComponent topPanel;

            public TopPanelAuthenticatedComponent topPanelAuthenticated;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }
    }
}