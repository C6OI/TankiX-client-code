using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1518618270341L)]
    public interface MoneyDailyBonusTemplate : ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        MoneyBonusComponent moneyBonus();
    }
}