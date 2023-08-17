using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-3936735916503799349L)]
    public interface VulcanBattleItemTemplate : Template, WeaponTemplate {
        VulcanComponent vulcan();

        [PersistentConfig]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();

        [PersistentConfig]
        VulcanWeaponComponent vulcanWeapon();

        KickbackComponent kickback();

        ImpactComponent impact();

        [AutoAdded]
        VulcanEnergyBarComponent vulcanEnergyBar();

        [PersistentConfig]
        DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator();

        [AutoAdded]
        VulcanWeaponStateComponent vulcanWeaponState();
    }
}