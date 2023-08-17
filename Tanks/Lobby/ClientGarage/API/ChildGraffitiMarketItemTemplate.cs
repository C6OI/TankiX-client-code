using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636100801497439942L)]
    public interface ChildGraffitiMarketItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate,
        GraffitiItemTemplate, GraffitiMarketItemTemplate, ItemImagedTemplate, MarketItemTemplate { }
}