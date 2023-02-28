using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatChannelComponent : Component {
        const int MAX_MESSAGES = 50;

        public ChatChannelComponent(ChatType chatType) {
            ChatType = chatType;
            Messages = new List<ChatMessage>();
        }

        public ChatType ChatType { get; }

        public List<ChatMessage> Messages { get; }

        public void AddMessage(ChatMessage message) {
            if (Messages.Count > 50) {
                Messages.RemoveAt(0);
            }

            Messages.Add(message);
        }
    }
}