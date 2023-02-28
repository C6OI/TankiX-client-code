using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class ShowNotificationEvent : Event {
        public ShowNotificationEvent(List<Entity> sortedNotifications) {
            CanShowNotification = true;
            SortedNotifications = sortedNotifications;
        }

        public bool CanShowNotification { get; set; }

        public List<Entity> SortedNotifications { get; }
    }
}