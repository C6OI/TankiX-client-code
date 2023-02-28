using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1486562494879L)]
    public interface GameplayChestUserItemTemplate : ContainerItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        GameplayChestItemComponent gameplayChestItem();

        [AutoAdded]
        [PersistentConfig]
        TargetTierComponent targetTier();
    }
}