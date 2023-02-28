using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1484905625943L)]
    public interface ModuleMarketItemTemplate : ModuleItemTemplate, MarketItemTemplate, GarageItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleUpgradePropertiesInfoComponent moduleUpgradePropertiesInfo();
    }
}