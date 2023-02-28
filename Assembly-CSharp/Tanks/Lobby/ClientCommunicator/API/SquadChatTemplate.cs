using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientCommunicator.Impl;

namespace Tanks.Lobby.ClientCommunicator.API {
    [SerialVersionUID(636479864244249445L)]
    public interface SquadChatTemplate : Template {
        ChatParticipantsComponent chatParticipants();

        [AutoAdded]
        SquadChatComponent squadChat();
    }
}