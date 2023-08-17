using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-6419489500262573655L)]
    public interface RailgunBattleItemTemplate : Template, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate,
        WeaponTemplate {
        RailgunComponent railgun();

        [PersistentConfig]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();

        RailgunChargingWeaponComponent chargingWeapon();

        ReadyRailgunChargingWeaponComponent readyRailgunChargingWeapon();

        PenetrationTargetCollectorComponent railgunTargetCollector();

        [AutoAdded]
        RailgunEnergyBarComponent railgunEnergyBar();
    }
}