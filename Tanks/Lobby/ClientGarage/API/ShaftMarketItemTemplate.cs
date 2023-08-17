using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435138382391L)]
    public interface ShaftMarketItemTemplate : Template, GarageItemTemplate, MarketItemTemplate, ShaftItemTemplate,
        WeaponItemTemplate, WeaponMarketItemTemplate { }
}