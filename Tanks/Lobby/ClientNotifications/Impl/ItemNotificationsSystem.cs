using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNotifications.API;
using UnityEngine;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class ItemNotificationsSystem : ECSSystem {
        [OnEventFire]
        public void CreateCommonNotification(NodeAddedEvent e, ScreenNode screen,
            NotActiveBaseItemNotificationNode notification, UserReadyForLobbyNode userReadyForLobby,
            SingleNode<NotificationsContainerComponent> notificationContainer) {
            if (screen.screen.ShowItemNotifications && !notification.itemNotification.created) {
                notification.itemNotification.created = true;
                GameObject gameObject = (GameObject)Object.Instantiate(notification.resourceData.Data);
                gameObject.transform.SetParent(notificationContainer.component.transform, false);
                EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
                component.BuildEntity(notification.Entity);
                screen.Entity.AddComponent<LockedScreenComponent>();
            }
        }

        [OnEventFire]
        public void CloseNotification(ItemNotificationCloseEvent e, BaseActiveItemNotificationNode notification,
            [JoinAll] SingleNode<LockedScreenComponent> screen) => CloseNotification(notification, screen.Entity);

        void CloseNotification(BaseActiveItemNotificationNode notification, Entity screen) {
            ScheduleEvent<ItemNotificationSeenEvent>(notification);
            Object.Destroy(notification.newIemNotification.gameObject);
            screen.RemoveComponent<LockedScreenComponent>();
        }

        public class UserReadyForLobbyNode : Node {
            public SelfUserComponent selfUser;

            public UserReadyForLobbyComponent userReadyForLobby;
        }

        public class BaseItemNotificationNode : Node {
            public ItemNotificationComponent itemNotification;

            public ResourceDataComponent resourceData;
        }

        [Not(typeof(NewIemNotificationComponent))]
        public class NotActiveBaseItemNotificationNode : BaseItemNotificationNode { }

        public class BaseActiveItemNotificationNode : BaseItemNotificationNode {
            public NewIemNotificationComponent newIemNotification;
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }
    }
}