using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

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