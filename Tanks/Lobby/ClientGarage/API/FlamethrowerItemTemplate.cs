using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714640358L)]
    public interface FlamethrowerItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [AutoAdded]
        [PersistentConfig]
        DamagePerSecondPropertyComponent damagePerSecondProperty();

        [AutoAdded]
        [PersistentConfig]
        TemperatureLimitPropertyComponent temperatureLimitProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyChargeSpeedPropertyComponent energyChargeSpeedProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyRechargeSpeedPropertyComponent energyRechargeSpeedProperty();

        [AutoAdded]
        [PersistentConfig]
        MinDamageDistancePropertyComponent minDamageDistanceProperty();
    }
}