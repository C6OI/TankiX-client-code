using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNotifications.API;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class UIDChangedNotificationSystem : ECSSystem {
        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, NotificationNode notification,
            [JoinAll] ChangeUIDSystem.SelfUserNode user) {
            string message = string.Format(notification.uIDChangedNotificationText.MessageTemplate, user.userUid.Uid);
            notification.Entity.AddComponent(new NotificationMessageComponent(message));
        }

        public class NotificationNode : Node {
            public UIDChangedNotificationComponent uIDChangedNotification;

            public UIDChangedNotificationTextComponent uIDChangedNotificationText;
        }
    }
}