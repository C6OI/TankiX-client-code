using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-2434344547754767853L)]
    public interface
        SmokyBattleItemTemplate : Template, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate {
        SmokyComponent smoky();

        [PersistentConfig]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();

        [AutoAdded]
        EnergyBarComponent energyBar();
    }
}