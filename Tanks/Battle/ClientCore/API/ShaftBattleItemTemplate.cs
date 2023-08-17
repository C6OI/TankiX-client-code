using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-2537616944465628484L)]
    public interface ShaftBattleItemTemplate : Template, WeaponTemplate {
        ShaftComponent shaft();

        DirectionEvaluatorComponent directionEvaluator();

        [PersistentConfig]
        DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator();

        ShaftStateControllerComponent shaftStateController();

        [PersistentConfig]
        ShaftStateConfigComponent shaftStateConfig();

        [PersistentConfig]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();

        [AutoAdded]
        EnergyBarComponent energyBar();

        KickbackComponent kickback();

        ImpactComponent impact();

        ShaftAimingImpactComponent shaftAimingImpact();

        ShaftEnergyComponent shaftEnergy();
    }
}