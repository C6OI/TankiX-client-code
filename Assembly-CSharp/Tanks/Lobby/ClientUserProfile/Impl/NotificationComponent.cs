using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1464339267328L)]
    [Shared]
    public class NotificationComponent : Component, IComparable<NotificationComponent> {
        public NotificationPriority Priority { get; set; }

        public Date TimeCreation { get; set; }

        public int CompareTo(NotificationComponent other) {
            int num = other.Priority.CompareTo(Priority);

            if (num == 0) {
                return TimeCreation.CompareTo(other.TimeCreation);
            }

            return num;
        }
    }
}