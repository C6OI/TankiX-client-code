using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class SendMessageEvent : Event {
        public SendMessageEvent(string message) => Message = message;

        public string Message { get; set; }
    }
}