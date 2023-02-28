using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.Impl;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1513602654661L)]
    public interface PersonalBattleRewardTemplate : Template {
        [AutoAdded]
        PersonalBattleRewardComponent personalBattleReward();

        BattleRewardGroupComponent battleRewardGroup();
    }
}