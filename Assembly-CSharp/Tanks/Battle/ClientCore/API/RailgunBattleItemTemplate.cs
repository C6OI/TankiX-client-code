using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-6419489500262573655L)]
    public interface RailgunBattleItemTemplate : DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template {
        RailgunComponent railgun();

        [PersistentConfig]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();

        RailgunChargingWeaponComponent chargingWeapon();

        ReadyRailgunChargingWeaponComponent readyRailgunChargingWeapon();

        [AutoAdded]
        RailgunEnergyBarComponent railgunEnergyBar();

        [AutoAdded]
        [PersistentConfig("reticle")]
        ReticleTemplateComponent reticleTemplate();
    }
}