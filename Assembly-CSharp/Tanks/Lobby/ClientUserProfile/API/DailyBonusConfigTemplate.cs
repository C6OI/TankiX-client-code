using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(636457280793479184L)]
    public interface DailyBonusConfigTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        DailyBonusFirstCycleComponent dailyBonusFirstCycle();

        [AutoAdded]
        [PersistentConfig]
        DailyBonusEndlessCycleComponent dailyBonusEndlessCycle();

        [AutoAdded]
        [PersistentConfig]
        DailyBonusCommonConfigComponent dailyBonusCommonConfig();
    }
}