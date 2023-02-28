using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class PaymentNotificationSystem : ECSSystem {
        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, PaymentNotificationNode notification) {
            notification.Entity.AddComponent(new NotificationMessageComponent {
                Message = string.Format(notification.paymentNotificationText.MessageTemplate, notification.paymentNotification.Amount)
            });
        }

        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, SpecialOfferNotificationNode notification) {
            notification.Entity.AddComponent(new NotificationMessageComponent {
                Message = string.Format(notification.paymentNotificationText.MessageTemplate, notification.specialOfferNotification.OfferName)
            });
        }

        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, SaleEndNotificationNode notification) {
            notification.Entity.AddComponent(new NotificationMessageComponent {
                Message = string.Format(notification.saleEndNotificationText.Message)
            });
        }

        public class PaymentNotificationNode : Node {
            public PaymentNotificationComponent paymentNotification;

            public PaymentNotificationTextComponent paymentNotificationText;

            public ResourceDataComponent resourceData;
        }

        public class SpecialOfferNotificationNode : Node {
            public PaymentNotificationTextComponent paymentNotificationText;

            public ResourceDataComponent resourceData;
            public SpecialOfferNotificationComponent specialOfferNotification;
        }

        public class SaleEndNotificationNode : Node {
            public ResourceDataComponent resourceData;
            public SaleEndNotificationTextComponent saleEndNotificationText;
        }
    }
}