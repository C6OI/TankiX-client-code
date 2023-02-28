using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientPayment.API {
    [SerialVersionUID(1467022824334L)]
    public interface PaymentNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        PaymentNotificationTextComponent paymentNotificationText();
    }
}