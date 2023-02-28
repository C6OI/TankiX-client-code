using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.API {
    [SerialVersionUID(1447137441472L)]
    public interface ChatTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        ChatConfigComponent chatConfig();

        ChatComponent Chat();
    }
}