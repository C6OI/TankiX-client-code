using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.API {
    [SerialVersionUID(1447137441472L)]
    public interface ChatTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        ChatConfigComponent chatConfig();

        ChatComponent Chat();
    }
}