using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1493972656490L)]
    public interface PresetMarketItemTemplate : PresetItemTemplate, MarketItemTemplate, GarageItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        FirstBuySaleComponent firstBuySale();

        [AutoAdded]
        [PersistentConfig]
        CreateByRankConfigComponent createByRankConfig();

        [AutoAdded]
        [PersistentConfig]
        ItemsBuyCountLimitComponent itemsBuyCountLimit();

        [AutoAdded]
        [PersistentConfig]
        ItemAutoIncreasePriceComponent itemsAutoIncreasePrice();
    }
}