using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435910107339L)]
    public interface IsisMarketItemTemplate : Template, GarageItemTemplate, MarketItemTemplate, IsisItemTemplate,
        WeaponItemTemplate, WeaponMarketItemTemplate { }
}