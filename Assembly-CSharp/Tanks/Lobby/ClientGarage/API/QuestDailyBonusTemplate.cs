using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1513768369538L)]
    public interface QuestDailyBonusTemplate : Template {
        [AutoAdded]
        QuestExchangeBonusComponent questExchangeBonus();
    }
}