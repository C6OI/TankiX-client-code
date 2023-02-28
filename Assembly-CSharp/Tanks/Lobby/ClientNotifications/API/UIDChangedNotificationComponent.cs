using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(1475754429807L)]
    public class UIDChangedNotificationComponent : Component {
        public string OldUserUID { get; set; }
    }
}