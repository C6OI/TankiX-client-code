using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-1716200834009238305L)]
    public interface DiscreteWeaponTemplate : WeaponTemplate, Template {
        [AutoAdded]
        WeaponHitStrongComponent weaponHitStrong();

        DiscreteWeaponControllerComponent discreteWeaponController();

        DirectionEvaluatorComponent directionEvaluator();

        [PersistentConfig]
        DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator();

        KickbackComponent kickback();

        ImpactComponent impact();

        DiscreteWeaponEnergyComponent discreteWeaponEnergy();
    }
}