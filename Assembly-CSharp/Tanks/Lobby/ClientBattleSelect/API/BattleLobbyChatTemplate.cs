using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientCommunicator.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1499421322354L)]
    public interface BattleLobbyChatTemplate : ChatTemplate, Template {
        [AutoAdded]
        BattleLobbyChatComponent battleLobbyChat();
    }
}