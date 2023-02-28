using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1430285417001L)]
    public interface WeaponTemplate : Template {
        TankPartComponent tankPart();

        WeaponComponent weapon();

        WeaponInstanceComponent weaponInstance();

        WeaponCooldownComponent weaponCooldown();

        CooldownTimerComponent cooldownTimer();

        TargetCollectorComponent targetCollector();

        [AutoAdded]
        ShotValidateComponent shotValidate();

        TeamTargetEvaluatorComponent teamTargetEvaluator();

        [PersistentConfig]
        CTFTargetEvaluatorComponent ctfTargetEvaluator();

        [AutoAdded]
        WeaponRotationControlComponent weaponRotationControl();

        [AutoAdded]
        WeaponGyroscopeRotationComponent weaponGyroscopeRotationComponent();

        [AutoAdded]
        [PersistentConfig("", true)]
        WeaponGyroscopeComponent weaponGyroscope();

        [AutoAdded]
        ShotIdComponent shotId();
    }
}