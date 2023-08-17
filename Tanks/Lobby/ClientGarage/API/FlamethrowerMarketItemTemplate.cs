using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435138131935L)]
    public interface FlamethrowerMarketItemTemplate : Template, GarageItemTemplate, MarketItemTemplate,
        FlamethrowerItemTemplate, WeaponItemTemplate, WeaponMarketItemTemplate { }
}