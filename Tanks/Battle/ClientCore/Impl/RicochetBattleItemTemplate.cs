using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-8939173357737272930L)]
    public interface RicochetBattleItemTemplate : Template, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate,
        WeaponTemplate {
        RicochetComponent ricochet();

        [PersistentConfig]
        VerticalTargetingComponent verticalTargeting();

        RicochetTargetCollectorComponent ricochetTargetCollector();

        [AutoAdded]
        EnergyBarComponent energyBar();
    }
}