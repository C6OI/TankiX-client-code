using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437713831380L)]
    public interface ShaftItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [AutoAdded]
        [PersistentConfig]
        AimingMaxDamagePropertyComponent aimingMaxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        AimingMinDamagePropertyComponent aimingMinDamageProperty();

        [PersistentConfig]
        [AutoAdded]
        ImpactPropertyComponent impactProperty();

        [AutoAdded]
        [PersistentConfig]
        ReloadTimePropertyComponent reloadTimeProperty();

        [AutoAdded]
        [PersistentConfig]
        EnergyChargePerShotPropertyComponent energyChargePerShotProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyChargeSpeedPropertyComponent energyChargeSpeedProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyRechargeSpeedPropertyComponent energyRechargeSpeedProperty();
    }
}