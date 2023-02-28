using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(7191911741927360956L)]
    public interface ShellItemTemplate : GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        ShellItemComponent shellItem();

        [AutoAdded]
        MountableItemComponent mountableItem();
    }
}