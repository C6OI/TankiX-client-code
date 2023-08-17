using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(583528765588657091L)]
    public interface
        TwinsBattleItemTemplate : Template, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate {
        TwinsComponent twins();

        [PersistentConfig]
        VerticalTargetingComponent verticalTargeting();

        [AutoAdded]
        EnergyBarComponent energyBar();
    }
}