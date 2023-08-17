using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Lobby.ClientUserProfile.src.main.csharp.Impl.Notifications.Component;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lobby.ClientUserProfile.Impl {
    public class NotificationSystem : ECSSystem {
        [OnEventFire]
        public void AddNotificationToQueue(NodeAddedEvent e, ReadyNotificationNode notification,
            [JoinAll] UserNotificationsGroupNode user) => user.notificationsGroup.Attach(notification.Entity);

        [OnEventFire]
        public void ShowOnChangeScreenOrLoadNotification(NodeAddedEvent e, ScreenNode screen,
            [Context] [Combine] NotificationNode notification, UserNotificationsGroupNode user2notActive,
            [JoinBy(typeof(NotificationsGroupComponent))]
            ICollection<NotActiveNotificationNode> notActiveNotifications,
            UserNotificationsGroupNode user2Active,
            [JoinBy(typeof(NotificationsGroupComponent))]
            ICollection<ActiveNotificationNode> activeNotifications) {
            if (screen.screen.ShowNotifications && activeNotifications.Count == 0 && notActiveNotifications.Count > 0) {
                ScheduleEvent<ShowNotificationEvent>(notActiveNotifications.First().Entity);
            }
        }

        [OnEventComplete]
        public void ShowNextNotification(NodeRemoveEvent e, ActiveNotificationNode notification,
            [JoinBy(typeof(NotificationsGroupComponent))]
            ICollection<NotActiveNotificationNode> notifications) {
            if (notifications.Count > 0) {
                List<NotActiveNotificationNode> list = notifications.ToList();
                list.Sort((a, b) => a.CompareTo(b));
                ScheduleEvent<ShowNotificationEvent>(notifications.First().Entity);
            }
        }

        [OnEventFire]
        public void ShowNotification(ShowNotificationEvent e, NotActiveNotificationNode notification,
            [JoinAll] SingleNode<NotificationsContainerComponent> notificationContainer) => CreateNotification(
            notification.Entity,
            notification.resourceData.Data,
            notificationContainer.component,
            notification.notificationConfig.ShowDuration);

        void CreateNotification(Entity notificationEntity, Object prefab,
            NotificationsContainerComponent notificationContainer, float timeToLive) {
            GameObject gameObject = (GameObject)Object.Instantiate(prefab);
            gameObject.transform.SetParent(notificationContainer.transform, false);
            EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
            component.BuildEntity(notificationEntity);

            if (timeToLive > 0f) {
                NewEvent<HideNotificationEvent>().Attach(notificationEntity).ScheduleDelayed(timeToLive);
            }
        }

        [OnEventFire]
        public void DestroyNotification(NodeRemoveEvent e, ReadyNotificationNode readyNotification,
            [JoinSelf] ActiveNotificationNode notification) => Object.Destroy(notification.activeNotification.gameObject);

        [OnEventFire]
        public void HideNotification(NodeAddedEvent e, ScreenNode screen, ActiveNotificationNode notification) {
            if (!screen.screen.ShowNotifications && notification.Entity.HasComponent<NotifficationMappingComponent>()) {
                notification.Entity.GetComponent<NotifficationMappingComponent>().enabled = false;
                notification.activeNotification.Hide();
            }
        }

        [OnEventFire]
        public void SetNotificationText(NodeAddedEvent e, ActiveNotificationNode notification) =>
            notification.activeNotification.Text.text = notification.notificationMessage.Message;

        [OnEventComplete]
        public void UpdateNotificationText(UpdateEvent e, UpdatedNotificationNode notification) =>
            notification.activeNotification.Text.text = notification.notificationMessage.Message;

        [OnEventFire]
        public void HideNotification(HideNotificationEvent e, ActiveNotificationNode notification) =>
            notification.activeNotification.Hide();

        [OnEventFire]
        public void NotificationShowed(NotificationClickEvent e, ClickableNotificationNode notification) =>
            notification.activeNotification.Hide();

        public class UserNotificationsGroupNode : Node {
            public NotificationsGroupComponent notificationsGroup;

            public SelfUserComponent selfUser;
        }

        public class ReadyNotificationNode : Node, IComparable<ReadyNotificationNode> {
            public NotificationComponent notification;

            public NotificationConfigComponent notificationConfig;

            public NotificationMessageComponent notificationMessage;

            public ResourceDataComponent resourceData;

            public int CompareTo(ReadyNotificationNode other) => notification.CompareTo(other.notification);
        }

        public class NotificationNode : ReadyNotificationNode {
            public NotificationsGroupComponent notificationsGroup;
        }

        public class ActiveNotificationNode : NotificationNode {
            public ActiveNotificationComponent activeNotification;
        }

        [Not(typeof(ActiveNotificationComponent))]
        public class NotActiveNotificationNode : NotificationNode { }

        public class UpdatedNotificationNode : ActiveNotificationNode {
            public UpdatedNotificationComponent updatedNotification;
        }

        [Not(typeof(NotClickableNotificationComponent))]
        public class ClickableNotificationNode : ActiveNotificationNode { }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }
    }
}