using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1433406763806L)]
    public interface SmokyMarketItemTemplate : Template, GarageItemTemplate, MarketItemTemplate, SmokyItemTemplate,
        WeaponItemTemplate, WeaponMarketItemTemplate { }
}