using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.Impl;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1513666142348L)]
    public interface XCrystalBonusPersonalBattleRewardTemplate : PersonalBattleRewardTemplate, Template {
        XCrystalBonusPersonalRewardComponent xCrystalBonusPersonalReward();
    }
}