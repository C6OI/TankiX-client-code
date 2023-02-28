using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientCommunicator.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1450340158222L)]
    public interface TeamBattleChatTemplate : ChatTemplate, Template { }
}