using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(1518597226923L)]
    public class EnergyCompensationNotificationComponent : Component {
        public long Charges { get; set; }

        public long Crys { get; set; }
    }
}