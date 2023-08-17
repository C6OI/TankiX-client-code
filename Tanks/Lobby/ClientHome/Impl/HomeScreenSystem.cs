using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientNavigation.Impl;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientHome.API;

namespace Tanks.Lobby.ClientHome.Impl {
    public class HomeScreenSystem : ECSSystem {
        [OnEventFire]
        public void ShowHomeScreen(NodeRemoveEvent e, SingleNode<PreloadAllResourcesComponent> preloadAllResourcesRequest,
            [JoinAll] TopPanelNode topPanel) => ScheduleEvent<ShowScreenNoAnimationEvent<HomeScreenComponent>>(topPanel);

        [OnEventFire]
        public void ShowGarageScreen(ButtonClickEvent e, SingleNode<GarageButtonComponent> node) =>
            ScheduleEvent<ShowScreenLeftEvent<GarageCategoryScreenComponent>>(node);

        [OnEventFire]
        public void ShowBattlesScreen(ButtonClickEvent e, SingleNode<BattlesButtonComponent> node) {
            Entity entity = CreateEntity("BattleSelectScreenContext");
            entity.AddComponent<BattleSelectScreenContextComponent>();
            ShowScreenLeftEvent<BattleSelectScreenComponent> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(entity, true);
            ScheduleEvent(showScreenLeftEvent, entity);
        }

        [OnEventFire]
        public void ShowSettingsScreen(ButtonClickEvent e, SingleNode<SettingsButtonComponent> node) =>
            ScheduleEvent<ShowScreenLeftEvent<SettingsScreenComponent>>(node);

        [OnEventFire]
        public void ShowFriendsScreen(ButtonClickEvent e, SingleNode<FriendsButtonComponent> node,
            [JoinAll] SingleNode<ScreenForegroundComponent> screenForeground) =>
            ScheduleEvent<ShowScreenLeftEvent<FriendsScreenComponent>>(node);

        [OnEventFire]
        public void GroupWithUser(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen, SelfUserNode selfUser) {
            ScheduleEvent(new SetScreenHeaderEvent {
                    Animate = false,
                    Header = string.Empty
                },
                homeScreen);

            homeScreen.component.UidText = selfUser.userUid.Uid.ToUpper();
            selfUser.userGroup.Attach(homeScreen.Entity);
        }

        [OnEventFire]
        public void FinalizeCompactWindow(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen) =>
            GraphicsSettings.INSTANCE.DisableCompactScreen();

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