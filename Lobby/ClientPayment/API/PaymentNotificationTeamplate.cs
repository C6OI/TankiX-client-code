using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.API {
    [SerialVersionUID(1467022824334L)]
    public interface PaymentNotificationTeamplate : NotificationTemplate, Template {
        [PersistentConfig]
        [AutoAdded]
        PaymentNotificationTextComponent paymentNotificationText();
    }
}