using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714721313L)]
    public interface IsisItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [PersistentConfig]
        [AutoAdded]
        DamagePerSecondPropertyComponent damagePerSecondProperty();

        [AutoAdded]
        [PersistentConfig]
        HealingPropertyComponent healingProperty();

        [PersistentConfig]
        [AutoAdded]
        SelfHealingPropertyComponent selfHealingProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyChargeSpeedPropertyComponent energyChargeSpeedProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyRechargeSpeedPropertyComponent energyRechargeSpeedProperty();
    }
}