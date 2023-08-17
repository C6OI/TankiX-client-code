using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore {
    [SerialVersionUID(-8770103861152493981L)]
    public interface ThunderBattleItemTemplate : Template, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate,
        WeaponTemplate {
        ThunderComponent thunder();

        [PersistentConfig]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();

        [PersistentConfig]
        SplashWeaponComponent splashWeapon();

        [AutoAdded]
        EnergyBarComponent energyBar();
    }
}