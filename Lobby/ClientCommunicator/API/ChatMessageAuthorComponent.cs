using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.API {
    [Shared]
    [SerialVersionUID(1447142210704L)]
    public class ChatMessageAuthorComponent : Component {
        public Entity Author { get; set; }
    }
}