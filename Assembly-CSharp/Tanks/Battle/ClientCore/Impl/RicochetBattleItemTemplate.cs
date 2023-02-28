using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-8939173357737272930L)]
    public interface RicochetBattleItemTemplate : DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template {
        RicochetComponent ricochet();

        [PersistentConfig]
        VerticalTargetingComponent verticalTargeting();

        [AutoAdded]
        EnergyBarComponent energyBar();

        [AutoAdded]
        [PersistentConfig("reticle")]
        ReticleTemplateComponent reticleTemplate();
    }
}