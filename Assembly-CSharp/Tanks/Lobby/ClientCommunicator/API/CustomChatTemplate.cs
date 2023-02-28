using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientCommunicator.Impl;

namespace Tanks.Lobby.ClientCommunicator.API {
    [SerialVersionUID(636451562982611325L)]
    public interface CustomChatTemplate : Template {
        ChatParticipantsComponent chatParticipants();

        [AutoAdded]
        CustomChatComponent customChat();
    }
}