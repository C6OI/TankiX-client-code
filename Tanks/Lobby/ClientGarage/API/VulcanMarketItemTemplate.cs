using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435138178392L)]
    public interface VulcanMarketItemTemplate : Template, GarageItemTemplate, MarketItemTemplate, VulcanItemTemplate,
        WeaponItemTemplate, WeaponMarketItemTemplate { }
}