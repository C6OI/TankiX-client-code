using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1433406732656L)]
    public interface TankMarketItemTemplate : TankItemTemplate, MarketItemTemplate, GarageItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        CrystalsPurchaseUserRankRestrictionComponent crystalsPurchaseUserRankRestriction();
    }
}