using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1434441563654L)]
    public interface TankItemTemplate : Template, GarageItemTemplate {
        [AutoAdded]
        TankItemComponent tankItem();

        [AutoAdded]
        MountableItemComponent mountableItem();

        [PersistentConfig("health")]
        [AutoAdded]
        HealthPropertyComponent healthProperty();

        [PersistentConfig("weight")]
        [AutoAdded]
        WeightPropertyComponent weightProperty();

        [PersistentConfig("speed")]
        [AutoAdded]
        SpeedPropertyComponent speedProperty();

        [PersistentConfig("acceleration")]
        [AutoAdded]
        AccelerationPropertyComponent accelerationProperty();

        [PersistentConfig("turnSpeed")]
        [AutoAdded]
        TurnSpeedPropertyComponent turnSpeedProperty();
    }
}