using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1434441563654L)]
    public interface TankItemTemplate : GarageItemTemplate, Template {
        [AutoAdded]
        TankItemComponent tankItem();

        [AutoAdded]
        MountableItemComponent mountableItem();

        [AutoAdded]
        [PersistentConfig("", true)]
        VisualPropertiesComponent visualProperties();
    }
}