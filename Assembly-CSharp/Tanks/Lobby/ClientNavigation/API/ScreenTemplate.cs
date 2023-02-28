using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNavigation.API {
    [SerialVersionUID(635718871423266257L)]
    public interface ScreenTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        ScreenHeaderTextComponent screenHeaderText();
    }
}