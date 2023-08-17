using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714406079L)]
    public interface RicochetItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [PersistentConfig]
        [AutoAdded]
        MinDamagePropertyComponent minDamageProperty();

        [PersistentConfig]
        [AutoAdded]
        MaxDamagePropertyComponent maxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ImpactPropertyComponent impactProperty();

        [AutoAdded]
        [PersistentConfig]
        ReloadTimePropertyComponent reloadTimeProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyChargePerShotPropertyComponent energyChargePerShotProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyRechargeSpeedPropertyComponent energyRechargeSpeedProperty();

        [PersistentConfig]
        [AutoAdded]
        BulletSpeedPropertyComponent bulletSpeedProperty();

        [AutoAdded]
        [PersistentConfig]
        MinDamageDistancePropertyComponent minDamageDistanceProperty();
    }
}