using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1493197354957L)]
    public class ServerNotificationMessageComponent : Component {
        public string Message { get; set; }
    }
}