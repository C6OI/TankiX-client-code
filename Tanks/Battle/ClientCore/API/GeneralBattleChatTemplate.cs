using Lobby.ClientCommunicator.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1450339998721L)]
    public interface GeneralBattleChatTemplate : ChatTemplate, Template { }
}