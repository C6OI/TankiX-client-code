using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636389912708135720L)]
    public interface CheatGameplayChestUserItemTemplate : GameplayChestUserItemTemplate, ContainerItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate,
        ItemImagedTemplate, Template { }
}