using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636066080038546444L)]
    public interface WindowModesTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        WindowModesComponent windowModes();
    }
}