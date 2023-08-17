using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(1446035595156L)]
    public class RequestEnterToPublicChatEvent : Event {
        public RequestEnterToPublicChatEvent() { }

        public RequestEnterToPublicChatEvent(long chatDescriptionId) => ChatDescriptionId = chatDescriptionId;

        public long ChatDescriptionId { get; set; }
    }
}