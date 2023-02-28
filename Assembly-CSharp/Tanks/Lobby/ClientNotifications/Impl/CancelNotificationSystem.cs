using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class CancelNotificationSystem : ECSSystem {
        [OnEventFire]
        public void InitCancelBehaviour(NodeAddedEvent e, SingleNode<CancelNotificationComponent> notification) {
            notification.component.Init(notification.Entity);
        }

        [OnEventFire]
        public void CloseActiveNotificationEvent(CloseNotificationEvent evt, SingleNode<CancelNotificationComponent> notification) {
            notification.component.enabled = false;
        }
    }
}