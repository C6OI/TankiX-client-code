using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Lobby.ClientPayment.Impl {
    public class PaymentNotificationSystem : ECSSystem {
        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, PaymentNotificationNode notification) =>
            notification.Entity.AddComponent(new NotificationMessageComponent {
                Message = string.Format(notification.paymentNotificationText.MessageTemplate,
                    notification.paymentNotification.Amount)
            });

        public class PaymentNotificationNode : Node {
            public PaymentNotificationComponent paymentNotification;

            public PaymentNotificationTextComponent paymentNotificationText;

            public ResourceDataComponent resourceData;
        }
    }
}