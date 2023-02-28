using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636413290399070700L)]
    public interface TutorialGameplayChestMarketItemTemplate : GameplayChestMarketItemTemplate, ContainerItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate,
        ItemImagedTemplate, Template { }
}