using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1504269596164L)]
    public interface EnergyDailyBonusTemplate : ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        EnergyBonusComponent energyBonus();
    }
}