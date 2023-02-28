using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1433418466220L)]
    public interface WeaponItemTemplate : GarageItemTemplate, Template {
        [AutoAdded]
        WeaponItemComponent weaponItem();

        [AutoAdded]
        MountableItemComponent mountableItem();

        [AutoAdded]
        [PersistentConfig]
        ItemIconComponent itemIcon();

        [AutoAdded]
        [PersistentConfig("", true)]
        VisualPropertiesComponent visualProperties();
    }
}