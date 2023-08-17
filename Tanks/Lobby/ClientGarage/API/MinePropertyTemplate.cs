using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1439558363099L)]
    public interface MinePropertyTemplate : Template, SupplyPropertyTemplate {
        [AutoAdded]
        [PersistentConfig]
        MineConfigComponent mineConfig();
    }
}