using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.Impl;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1514196284686L)]
    public interface LevelUpUnlockBattleRewardTemplate : BattleResultRewardTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        LevelUpUnlockRewardConfigComponent levelUpUnlock();
    }
}