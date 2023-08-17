using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(635895920978852780L)]
    public interface ItemPropertyTemplate : Template {
        [PersistentConfig]
        ArmorSupplyPropertyComponent armorSupplyProperty();

        [PersistentConfig]
        DamageSupplyPropertyComponent damageSupplyProperty();

        [PersistentConfig]
        DependentCooldownSupplyPropertyComponent dependentCooldownSupplyProperty();

        [PersistentConfig]
        DurationSupplyPropertyComponent durationSupplyProperty();

        [PersistentConfig]
        MineDamageSupplyPropertyComponent mineDamageSupplyProperty();

        [PersistentConfig]
        RepairSupplyPropertyComponent repairSupplyProperty();

        [PersistentConfig]
        SelfCooldownSupplyPropertyComponent selfCooldownSupplyProperty();

        [PersistentConfig]
        SpeedSupplyPropertyComponent speedSupplyProperty();

        [PersistentConfig]
        WeaponRotationSpeedSupplyPropertyComponent weaponRotationSpeedSupplyProperty();

        [PersistentConfig]
        AccelerationGarageItemPropertyComponent accelerationGarageItemProperty();

        [PersistentConfig]
        HealthGarageItemPropertyComponent healthGarageItemProperty();

        [PersistentConfig]
        SpeedGarageItemPropertyComponent speedGarageItemProperty();

        [PersistentConfig]
        TurnSpeedGarageItemPropertyComponent turnSpeedGarageItemProperty();

        [PersistentConfig]
        WeightGarageItemPropertyComponent weightGarageItemProperty();

        [PersistentConfig]
        BulletSpeedGarageItemPropertyComponent bulletSpeedGarageItemProperty();

        [PersistentConfig]
        DamageDistanceGarageItemPropertyComponent damageDistanceGarageItemProperty();

        [PersistentConfig]
        DamageGarageItemPropertyComponent damageGarageItemProperty();

        [PersistentConfig]
        DamageWeakeningByTargetGarageItemPropertyComponent damageWeakeningByTargetGarageItemProperty();

        [PersistentConfig]
        EnergyChargeSpeedGarageItemPropertyComponent energyChargeSpeedGarageItemProperty();

        [PersistentConfig]
        EnergyRechargeSpeedGarageItemPropertyComponent energyRechargeSpeedGarageItemProperty();

        [PersistentConfig]
        FireDamageGarageItemPropertyComponent fireDamageGarageItemProperty();

        [PersistentConfig]
        ImpactGarageItemPropertyComponent impactGarageItemProperty();

        [PersistentConfig]
        ReloadTimeGarageItemPropertyComponent reloadTimeGarageItemProperty();

        [PersistentConfig]
        TurretTurnSpeedGarageItemPropertyComponent turretTurnSpeedGarageItemProperty();

        [PersistentConfig]
        CriticalDamageGarageItemPropertyComponent criticalDamageGarageItemProperty();

        [PersistentConfig]
        CriticalProbabilityGarageItemPropertyComponent criticalProbabilityGarageItemProperty();

        [PersistentConfig]
        MagazineSizeGarageItemPropertyComponent magazineSizeGarageItemProperty();

        [PersistentConfig]
        ReloadMagazineTimeGarageItemPropertyComponent reloadMagazineTimeGarageItemProperty();

        [PersistentConfig]
        HealingGarageItemPropertyComponent healingGarageItemProperty();

        [PersistentConfig]
        SelfHealingGarageItemPropertyComponent selfHealingGarageItemProperty();

        [PersistentConfig]
        AimingDamageGarageItemPropertyComponent aimingDamageGarageItemProperty();

        [PersistentConfig]
        ShaftDamageGarageItemPropertyComponent shaftDamageGarageItemProperty();

        [PersistentConfig]
        SpinUpTimeGarageItemPropertyComponent spinUpTimeGarageItemProperty();

        [PersistentConfig]
        TemperatureHittingTimeGarageItemPropertyComponent temperatureHittingTimeGarageItemProperty();

        [PersistentConfig]
        AccelerationUnitItemPropertyComponent symbolAccelerationItemProperty();

        [PersistentConfig]
        AngularVelocityUnitItemPropertyComponent symbolAngularVelocityItemProperty();

        [PersistentConfig]
        PercentUnitItemPropertyComponent symbolPercentItemProperty();

        [PersistentConfig]
        SecondUnitItemPropertyComponent symbolSecondItemProperty();

        [PersistentConfig]
        VelocityUnitItemPropertyComponent symbolVelocityItemProperty();

        [PersistentConfig]
        DistanceUnitItemPropertyComponent symbolDistanceItemProperty();

        [PersistentConfig]
        DependentSpeedCooldownSupplyPropertyComponent dependentSpeedCooldownSupplyProperty();

        [PersistentConfig]
        DependentArmorCooldownSupplyPropertyComponent dependentArmorCooldownSupplyProperty();

        [PersistentConfig]
        DependentDamageCooldownSupplyPropertyComponent dependentDamageCooldownSupplyProperty();

        [PersistentConfig]
        DependentMineCooldownSupplyPropertyComponent dependentMineCooldownSupplyProperty();

        [PersistentConfig]
        DependentRepairCooldownSupplyPropertyComponent dependentRepairCooldownSupplyProperty();
    }
}