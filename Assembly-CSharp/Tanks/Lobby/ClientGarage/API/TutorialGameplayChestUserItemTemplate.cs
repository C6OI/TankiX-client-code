using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636413315444096863L)]
    public interface TutorialGameplayChestUserItemTemplate : GameplayChestUserItemTemplate, ContainerItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate,
        ItemImagedTemplate, Template {
        [AutoAdded]
        HiddenInGarageItemComponent hiddenInGarageItem();
    }
}