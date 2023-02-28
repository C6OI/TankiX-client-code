using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(635908807309467349L)]
    public interface WeaponMarketItemTemplate : MarketItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        CrystalsPurchaseUserRankRestrictionComponent crystalsPurchaseUserRankRestriction();
    }
}