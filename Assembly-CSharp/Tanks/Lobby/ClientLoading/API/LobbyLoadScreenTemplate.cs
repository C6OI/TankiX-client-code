using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientLoading.API {
    [SerialVersionUID(8853284213578L)]
    public interface LobbyLoadScreenTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        LoadScreenLocalizedTextComponent loadScreenLocalizedText();
    }
}