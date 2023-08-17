using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientLoading.API {
    [SerialVersionUID(2342575478583L)]
    public interface PreloadAllResourcesScreenTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        LoadScreenLocalizedTextComponent loadScreenLocalizedText();
    }
}