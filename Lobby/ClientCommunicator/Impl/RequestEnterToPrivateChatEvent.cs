using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(1447136322547L)]
    public class RequestEnterToPrivateChatEvent : Event {
        public RequestEnterToPrivateChatEvent() { }

        public RequestEnterToPrivateChatEvent(long userId) => UserId = userId;

        public long UserId { get; set; }
    }
}