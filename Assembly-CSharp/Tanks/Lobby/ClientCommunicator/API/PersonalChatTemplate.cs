using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientCommunicator.Impl;

namespace Tanks.Lobby.ClientCommunicator.API {
    [SerialVersionUID(636469998634338659L)]
    public interface PersonalChatTemplate : Template {
        ChatParticipantsComponent chatParticipants();

        [AutoAdded]
        PersonalChatComponent personalChat();
    }
}