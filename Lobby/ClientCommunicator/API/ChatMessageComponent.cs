using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.API {
    [SerialVersionUID(1446035869638L)]
    [Shared]
    public class ChatMessageComponent : Component {
        public string Message { get; set; }
    }
}