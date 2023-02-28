using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [SerialVersionUID(1452772088812L)]
    public interface RanksNamesTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        RanksNamesComponent ranksNames();
    }
}