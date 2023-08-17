using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientCommunicator.Impl {
    public class PublicChatRequestComponent : Component {
        public PublicChatRequestComponent(long id, string name) {
            ChatDescripionId = id;
            ChatName = name;
        }

        public long ChatDescripionId { get; set; }

        public string ChatName { get; set; }
    }
}