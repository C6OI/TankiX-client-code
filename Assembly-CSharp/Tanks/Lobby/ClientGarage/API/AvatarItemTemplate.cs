using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1544694213157L)]
    public interface AvatarItemTemplate : GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        AvatarItemComponent avatarItem();

        [AutoAdded]
        MountableItemComponent mountableItem();
    }
}