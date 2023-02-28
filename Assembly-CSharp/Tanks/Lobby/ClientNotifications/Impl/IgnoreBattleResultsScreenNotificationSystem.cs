using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class IgnoreBattleResultsScreenNotificationSystem : ECSSystem {
        [OnEventFire]
        public void SkipNotificationOnBattleScreen(ShowNotificationEvent evt, SingleNode<IgnoreBattleResultScreenNotificationComponent> notification,
            [JoinAll] BattleResultsScreenNode screen) {
            evt.CanShowNotification = false;
        }

        [OnEventFire]
        public void CloseNotificationOnBattleScreen(NodeAddedEvent evt, [Combine] ActiveNotificationNode notification, BattleResultsScreenNode screen) {
            ScheduleEvent<CloseNotificationEvent>(notification);
        }

        [OnEventFire]
        public void SetScreenPartIndex(NodeAddedEvent evt, [Combine] UserRankRewardNotificationNode notification, BattleResultsScreenNode screen) {
            notification.ignoreBattleResultScreenNotification.ScreenPartIndex = 1;
        }

        [OnEventFire]
        public void SetScreenPartIndex(NodeAddedEvent evt, [Combine] NewItemCardNotificationNode notification, BattleResultsScreenNode screen) {
            notification.Entity.RemoveComponent<IgnoreBattleResultScreenNotificationComponent>();
        }

        [OnEventFire]
        public void SetScreenPartIndex(NodeAddedEvent evt, [Combine] NewItemNotificationNode notification, BattleResultsScreenNode screen) {
            IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotificationComponent = new();
            ignoreBattleResultScreenNotificationComponent.ScreenPartIndex = 1;
            IgnoreBattleResultScreenNotificationComponent component = ignoreBattleResultScreenNotificationComponent;
            notification.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void NewLeagueNotif(NodeAddedEvent e, SingleNode<LeagueFirstEntranceRewardNotificationComponent> notification) {
            notification.Entity.AddComponent(new IgnoreBattleResultScreenNotificationComponent {
                ScreenPartIndex = 1
            });
        }

        [OnEventFire]
        public void ShowIgnoredNotification(ShowBattleResultsScreenNotificationEvent e, Node any, [JoinAll] UserNode user,
            [JoinBy(typeof(NotificationsGroupComponent))]
            ICollection<SingleNode<IgnoreBattleResultScreenNotificationComponent>> notifications) {
            if (notifications.Count != 0 && !notifications.All(n => n.component.ScreenPartIndex != e.Index)) {
                SingleNode<IgnoreBattleResultScreenNotificationComponent> singleNode = notifications.First(n => n.component.ScreenPartIndex == e.Index);

                if (singleNode != null) {
                    e.NotificationExist = true;
                    singleNode.Entity.RemoveComponent<IgnoreBattleResultScreenNotificationComponent>();
                    NewEvent<TryToShowNotificationEvent>().Attach(user).ScheduleDelayed(0.3f);
                }
            }
        }

        [OnEventFire]
        public void IgnoredNotificationShown(NodeRemoveEvent e, ActiveReadyNotificationWithGroupNode notification, [JoinBy(typeof(NotificationsGroupComponent))] UserNode user) {
            NewEvent<ScreenPartShownEvent>().Attach(user).Schedule();
        }

        public class UserNode : Node {
            public NotificationsGroupComponent notificationsGroup;

            public SelfUserComponent selfUser;
        }

        public class BattleResultsScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public BattleResultScreenComponent battleResultScreen;
            public ScreenComponent screen;
        }

        public class ActiveNotificationNode : Node {
            public ActiveNotificationComponent activeNotification;

            public IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotification;
        }

        public class UserRankRewardNotificationNode : Node {
            public IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotification;
            public UserRankRewardNotificationInfoComponent userRankRewardNotificationInfo;
        }

        public class NewItemNotificationNode : Node {
            public NewItemNotificationComponent newItemNotification;
        }

        public class NewItemCardNotificationNode : Node {
            public IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotification;

            public NewCardItemNotificationComponent newCardItemNotification;
            public NewItemNotificationComponent newItemNotification;
        }

        public class ActiveReadyNotificationWithGroupNode : Node {
            public ActiveNotificationComponent activeNotification;
            public NotificationComponent notification;

            public NotificationsGroupComponent notificationsGroup;
        }
    }
}