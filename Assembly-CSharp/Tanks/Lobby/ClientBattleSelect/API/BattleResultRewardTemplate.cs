using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1513235522063L)]
    public interface BattleResultRewardTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        DescriptionItemComponent descriptionItem();

        BattleRewardGroupComponent battleRewardGroup();
    }
}