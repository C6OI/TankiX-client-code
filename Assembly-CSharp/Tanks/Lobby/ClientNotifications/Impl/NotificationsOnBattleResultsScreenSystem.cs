using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class NotificationsOnBattleResultsScreenSystem : ECSSystem {
        [OnEventFire]
        public void NewNotificationsOpened(NodeAddedEvent e, SingleNode<NewItemNotificationComponent> notification, [JoinAll] SingleNode<BattleResultsAwardsScreenComponent> screen) {
            screen.component.CardsCount++;
            screen.component.ShowNotiffication();
        }

        [OnEventFire]
        public void NewCardNotificationsClosed(CloseNotificationEvent e, ActiveCardNotificationNode cards, [JoinAll] SingleNode<BattleResultsAwardsScreenComponent> screen) {
            screen.component.CardsCount--;
            screen.component.HideNotiffication();
        }

        public class NotificationNode : Node {
            public NewItemNotificationComponent newItemNotification;

            public NewItemNotificationTextComponent newItemNotificationText;

            public NewPaintItemNotificationTextComponent newPaintItemNotificationText;
        }

        public class ActiveNotificationNode : NotificationNode {
            public ActiveNotificationComponent activeNotification;

            public NewItemNotificationUnityComponent newItemNotificationUnity;
        }

        public class ActiveCardNotificationNode : ActiveNotificationNode {
            public NewCardItemNotificationComponent newCardItemNotification;
        }

        [Not(typeof(NewCardItemNotificationComponent))]
        public class ActiveItemNotificationNode : ActiveNotificationNode { }
    }
}