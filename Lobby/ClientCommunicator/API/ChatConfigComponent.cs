using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientCommunicator.API {
    public class ChatConfigComponent : Component {
        public int MaxMessageLength { get; set; }
    }
}