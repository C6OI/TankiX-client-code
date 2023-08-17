using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435138774158L)]
    public interface HammerMarketItemTemplate : Template, GarageItemTemplate, MarketItemTemplate, HammerItemTemplate,
        WeaponItemTemplate, WeaponMarketItemTemplate { }
}