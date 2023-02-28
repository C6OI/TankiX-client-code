using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientPayment.API {
    [SerialVersionUID(1481785769449L)]
    public interface SaleEndNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        SaleEndNotificationTextComponent saleEndNotificationText();
    }
}