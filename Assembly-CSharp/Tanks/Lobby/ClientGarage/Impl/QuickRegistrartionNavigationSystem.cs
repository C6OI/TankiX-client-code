using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPaymentGUI.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class QuickRegistrartionNavigationSystem : ECSSystem {
        [OnEventComplete]
        public void ShowOnMainScreen(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen, UserWithIncompleteRegNode userWithIncompleteReg) {
            if (IsRegistartionTime(userWithIncompleteReg)) {
                NewEvent<DelayedShowRegistrationEvent>().Attach(homeScreen).ScheduleDelayed(0f);
            }
        }

        [OnEventFire]
        public void ShowOnMainScreenDelayed(DelayedShowRegistrationEvent e, SingleNode<MainScreenComponent> homeScreen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg,
            [JoinAll] ICollection<SingleNode<ActiveNotificationComponent>> activeNotifications) {
            if (IsRegistartionTime(userWithIncompleteReg) && activeNotifications.Count == 0) {
                NewEvent<ShowScreenDownEvent<RegistrationScreenComponent>>().Attach(homeScreen).ScheduleDelayed(0f);
            }
        }

        [OnEventComplete]
        public void ShowOnMainScreenAndNotificationLeave(NodeRemoveEvent e, SingleNode<ActiveNotificationComponent> activeNotification,
            [JoinAll] SingleNode<MainScreenComponent> homeScreen, UserWithIncompleteRegNode userWithIncompleteReg,
            [JoinAll] ICollection<SingleNode<ActiveNotificationComponent>> activeNotifications) {
            if (IsRegistartionTime(userWithIncompleteReg) && activeNotifications.Count == 1) {
                NewEvent<ShowScreenDownEvent<RegistrationScreenComponent>>().Attach(homeScreen).ScheduleDelayed(0f);
            }
        }

        [OnEventFire]
        public void Complete(NodeRemoveEvent e, SingleNode<UserIncompleteRegistrationComponent> user, [JoinAll] SingleNode<RegistrationScreenComponent> screen) {
            NewEvent<ShowScreenDownEvent<HomeScreenComponent>>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void Complete(ButtonClickEvent e, SingleNode<BackButtonComponent> user, [JoinAll] SingleNode<RegistrationScreenComponent> screen,
            [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg) {
            MainScreenComponent.Instance.ShowMain();
            NewEvent<ShowScreenDownEvent<HomeScreenComponent>>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void DisableShop(NodeAddedEvent e, SingleNode<ShopComponent> homeScreen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg) {
            MainScreenComponent.Instance.ShowMain();
        }

        [OnEventFire]
        public void ShowOnShopUserProfileDelayed(NodeAddedEvent e, SingleNode<ProfileScreenComponent> homeScreen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg) {
            NewEvent<ShowScreenDownEvent<RegistrationScreenComponent>>().Attach(homeScreen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowOnShopScreenDelayed(NodeAddedEvent e, SingleNode<ShopComponent> homeScreen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg) {
            NewEvent<ShowScreenDownEvent<RegistrationScreenComponent>>().Attach(homeScreen).ScheduleDelayed(0f);
        }

        public bool IsRegistartionTime(UserWithIncompleteRegNode userWithIncompleteReg) => userWithIncompleteReg.userIncompleteRegistration.FirstBattleDone;

        public class UserWithIncompleteRegNode : Node {
            public SelfUserComponent selfUser;
            public UserIncompleteRegistrationComponent userIncompleteRegistration;

            public UserRankComponent userRank;
        }

        public class DelayedShowRegistrationEvent : Event { }
    }
}