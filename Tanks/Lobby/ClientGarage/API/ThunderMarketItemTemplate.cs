using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435139147319L)]
    public interface ThunderMarketItemTemplate : Template, GarageItemTemplate, MarketItemTemplate, ThunderItemTemplate,
        WeaponItemTemplate, WeaponMarketItemTemplate { }
}