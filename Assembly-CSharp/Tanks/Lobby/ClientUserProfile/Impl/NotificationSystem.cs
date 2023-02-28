using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class NotificationSystem : ECSSystem {
        const float TIMEOUT = 0.05f;

        [OnEventFire]
        public void AddNotificationToQueue(NodeAddedEvent e, ReadyNotificationNode notification, [JoinAll] UserNotificationsGroupNode user) {
            Log.InfoFormat("AddNotificationToQueue notification={0}", notification);
            user.notificationsGroup.Attach(notification.Entity);
        }

        [OnEventFire]
        public void ShowOnChangeScreenOrLoadNotification(NodeAddedEvent e, ScreenNode screen, [Context] [Combine] ReadyNotificationWithGroupNode notification,
            UserNotificationsGroupNode user) {
            if (screen.screen.ShowNotifications) {
                NewEvent<TryToShowNotificationEvent>().Attach(user).ScheduleDelayed(0.05f);
            }
        }

        [OnEventFire]
        public void TryToShowNotification(TryToShowNotificationEvent evt, UserNotificationsGroupNode user1,
            [JoinBy(typeof(NotificationsGroupComponent))]
            ICollection<NotActiveReadyNotificationWithGroupNode> notActiveNotifications, UserNotificationsGroupNode user2,
            [JoinBy(typeof(NotificationsGroupComponent))]
            ICollection<ActiveReadyNotificationWithGroupNode> activeNotifications, [JoinAll] NotLockedScreenNode screen) {
            if (Log.IsInfoEnabled) {
                Log.InfoFormat("TryToShowNotification activeNotifications={0} notActiveNotifications={1}", EcsToStringUtil.EnumerableToString(activeNotifications),
                    EcsToStringUtil.EnumerableToString(notActiveNotifications));
            }

            if ((activeNotifications.Count <= 0 || !activeNotifications.Any((ActiveReadyNotificationWithGroupNode n) => n.activeNotification.Visible)) &&
                notActiveNotifications.Count > 0) {
                List<Entity> list = SortNonActiveNotifications(notActiveNotifications);
                ScheduleEvent(new ShowNotificationEvent(list), list.First());
            }
        }

        List<Entity> SortNonActiveNotifications(ICollection<NotActiveReadyNotificationWithGroupNode> notActiveNotifications) {
            List<NotActiveReadyNotificationWithGroupNode> list = notActiveNotifications.ToList();
            list.Sort((NotActiveReadyNotificationWithGroupNode a, NotActiveReadyNotificationWithGroupNode b) => a.notificationConfig.Order.CompareTo(b.notificationConfig.Order));
            return list.Select((NotActiveReadyNotificationWithGroupNode n) => n.Entity).ToList();
        }

        [OnEventComplete]
        public void ShowNextNotification(NodeRemoveEvent e, ActiveReadyNotificationWithGroupNode notification,
            [JoinBy(typeof(NotificationsGroupComponent))]
            UserNotificationsGroupNode user) {
            NewEvent<TryToShowNotificationEvent>().Attach(user).ScheduleDelayed(0.05f);
        }

        [OnEventComplete]
        public void ShowNotification(ShowNotificationEvent e, NotActiveReadyNotificationWithGroupNode notification,
            [JoinBy(typeof(NotificationGroupComponent))]
            ICollection<NotActiveReadyNotificationWithGroupNode> notifications,
            [JoinAll] SingleNode<NotificationsContainerComponent> notificationContainer, [JoinAll] ScreenNode screen) {
            if (Log.IsInfoEnabled) {
                Log.InfoFormat("ShowNotification {0} CanShowNotification={1}", notification, e.CanShowNotification);
            }

            if (e.CanShowNotification) {
                e.SortedNotifications.Clear();

                if (notifications.Any()) {
                    IEnumerable<NotActiveReadyNotificationWithGroupNode> enumerable = notifications.Take(notificationContainer.component.MaxItemsPerScreen);
                    int num = 0;

                    {
                        foreach (NotActiveReadyNotificationWithGroupNode item in enumerable) {
                            CreateNotificationObject(item, notificationContainer.component, num, notifications.Count);
                            num++;
                        }

                        return;
                    }
                }

                CreateNotificationObject(notification, notificationContainer.component, 0, 1);
            } else {
                List<Entity> sortedNotifications = e.SortedNotifications;
                sortedNotifications.Remove(notification.Entity);

                if (sortedNotifications.Count != 0) {
                    ScheduleEvent(new ShowNotificationEvent(sortedNotifications), sortedNotifications.First());
                }
            }
        }

        void CreateNotificationObject(NotActiveReadyNotificationWithGroupNode notification, NotificationsContainerComponent notificationContainer, int index,
            int notificationsCount) {
            GameObject gameObject = Object.Instantiate(notification.resourceData.Data) as GameObject;

            gameObject.transform.SetParent(
                !notification.notificationConfig.IsFullScreen ? notificationContainer.GetParenTransform(index, notificationsCount)
                    : notificationContainer.GetFullSceenNotificationContainer(), false);

            notification.Entity.AddComponent(new NotificationInstanceComponent(gameObject));

            gameObject.GetComponentsInChildren<ActiveNotificationComponent>(true).ForEach(delegate(ActiveNotificationComponent i) {
                i.Entity = notification.Entity;
            });

            EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
            component.BuildEntity(notification.Entity);
            float showDuration = notification.notificationConfig.ShowDuration;
            float showDelay = notification.notificationConfig.ShowDelay;
            NewEvent<SetNotificationVisibleEvent>().Attach(notification.Entity).ScheduleDelayed((float)index * showDelay);

            if (showDuration > 0f) {
                NewEvent<HideNotificationEvent>().Attach(notification.Entity).ScheduleDelayed(showDuration);
            }
        }

        [OnEventFire]
        public void DestroyNotification(NodeRemoveEvent e, SingleNode<NotificationInstanceComponent> notification) {
            Object.Destroy(notification.component.Instance);
        }

        [OnEventFire]
        public void HideNotification(NodeAddedEvent e, ScreenNode screen, [Combine] ActiveReadyNotificationMappingWithGroupNode notification) {
            if (!screen.screen.ShowNotifications) {
                ScheduleEvent<CloseNotificationEvent>(notification);
            }
        }

        [OnEventFire]
        public void SetNotificationVisible(SetNotificationVisibleEvent e, ActiveReadyNotificationWithGroupNode notification) {
            notification.activeNotification.Show();
        }

        [OnEventFire]
        public void CloseActiveNotificationEvent(CloseNotificationEvent evt, ActiveReadyNotificationMappingWithGroupNode notification) {
            notification.notifficationMapping.enabled = false;
        }

        [OnEventComplete]
        public void CloseActiveNotificationEvent(CloseNotificationEvent evt, ActiveReadyNotificationWithGroupNode notification) {
            notification.activeNotification.Hide();
        }

        [OnEventFire]
        public void HideNotification(HideNotificationEvent e, ActiveReadyNotificationWithGroupNode notification) {
            ScheduleEvent<CloseNotificationEvent>(notification);
        }

        [OnEventComplete]
        public void HideNotification(NotificationClickEvent e, ClickableReadyNotificationWithGroupNode notification) {
            ScheduleEvent<CloseNotificationEvent>(notification);
        }

        [OnEventFire]
        public void SetNotificationText(NodeAddedEvent e, NotificationWithMessageNode notification) {
            notification.activeNotification.Text.text = notification.notificationMessage.Message;
        }

        [OnEventComplete]
        public void UpdateNotificationText(UpdateEvent e, UpdatedReadyNotificationWithMessageNode notification) {
            notification.activeNotification.Text.text = notification.notificationMessage.Message;
        }

        [OnEventFire]
        public void AddMessage(NodeAddedEvent e, SingleNode<ServerNotificationMessageComponent> notification) {
            notification.Entity.AddComponent(new NotificationMessageComponent(notification.component.Message));
        }

        public class UserNotificationsGroupNode : Node {
            public NotificationsGroupComponent notificationsGroup;

            public SelfUserComponent selfUser;
        }

        public class ReadyNotificationNode : Node, IComparable<ReadyNotificationNode> {
            public NotificationComponent notification;

            public NotificationConfigComponent notificationConfig;

            public ResourceDataComponent resourceData;

            public int CompareTo(ReadyNotificationNode other) => notification.CompareTo(other.notification);
        }

        public class ReadyNotificationWithGroupNode : ReadyNotificationNode {
            public NotificationsGroupComponent notificationsGroup;
        }

        public class ActiveReadyNotificationWithGroupNode : ReadyNotificationWithGroupNode {
            public ActiveNotificationComponent activeNotification;
        }

        public class ActiveReadyNotificationMappingWithGroupNode : ActiveReadyNotificationWithGroupNode {
            public NotifficationMappingComponent notifficationMapping;
        }

        [Not(typeof(ActiveNotificationComponent))]
        public class NotActiveReadyNotificationWithGroupNode : ReadyNotificationWithGroupNode { }

        [Not(typeof(NotClickableNotificationComponent))]
        public class ClickableReadyNotificationWithGroupNode : ActiveReadyNotificationWithGroupNode { }

        public class NotificationWithMessageNode : ReadyNotificationNode {
            public ActiveNotificationComponent activeNotification;

            public NotificationMessageComponent notificationMessage;
        }

        public class UpdatedReadyNotificationWithMessageNode : NotificationWithMessageNode {
            public UpdatedNotificationComponent updatedNotification;
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        [Not(typeof(LockedScreenComponent))]
        public class NotLockedScreenNode : ScreenNode { }
    }
}