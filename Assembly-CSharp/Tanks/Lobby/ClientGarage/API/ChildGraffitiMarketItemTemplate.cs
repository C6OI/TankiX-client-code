using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636100801497439942L)]
    public interface ChildGraffitiMarketItemTemplate : GraffitiMarketItemTemplate, GraffitiItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate,
        ItemImagedTemplate, Template { }
}