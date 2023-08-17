using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1469607967377L)]
    public interface HullSkinMarketItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        MarketItemTemplate, HullSkinItemTemplate, SkinItemTemplate, SkinMarketItemTemplate { }
}