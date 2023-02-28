using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(636117026898825840L)]
    public interface UserNotificatorRankNamesTemplate : Template {
        [AutoAdded]
        UserNotificatorRankNamesComponent userNotificatorRankNames();

        [AutoAdded]
        [PersistentConfig]
        RanksNamesComponent ranksNames();
    }
}