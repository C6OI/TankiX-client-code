using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(4939169559170921259L)]
    public interface HammerBattleItemTemplate : Template, DiscreteWeaponTemplate, WeaponTemplate {
        HammerComponent hammer();

        WeaponShotComponent shot();

        [PersistentConfig]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();

        [PersistentConfig]
        HammerPelletConeComponent hammerPelletCone();

        [AutoAdded]
        HammerEnergyBarComponent hammerEnergyBarComponent();
    }
}