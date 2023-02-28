using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(1450950140104L)]
    public class ChatMessageReceivedEvent : Event {
        public string Message { get; set; }

        public bool SystemMessage { get; set; }

        public string UserUid { get; set; }

        public long UserId { get; set; }

        public string UserAvatarId { get; set; }
    }
}