using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class DiscreteWeaponControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void StartShotIfPossible(EarlyUpdateEvent evt, SelfTankNode selfTank, [JoinByTank] DiscreteWeaponControllerNode discreteWeaponEnergyController) {
            float unloadEnergyPerShot = discreteWeaponEnergyController.discreteWeaponEnergy.UnloadEnergyPerShot;
            float energy = discreteWeaponEnergyController.weaponEnergy.Energy;
            CooldownTimerComponent cooldownTimer = discreteWeaponEnergyController.cooldownTimer;

            if (!(energy < unloadEnergyPerShot) && !(cooldownTimer.CooldownTimerSec > 0f) && InputManager.GetAxisOrKey(ShotActions.SHOT) != 0f) {
                ScheduleEvent<BeforeShotEvent>(discreteWeaponEnergyController);
                ScheduleEvent<ShotPrepareEvent>(discreteWeaponEnergyController);
                ScheduleEvent<PostShotEvent>(discreteWeaponEnergyController);
            }
        }

        [OnEventFire]
        public void StartShotIfPossible(EarlyUpdateEvent evt, SelfTankNode selfTank, [JoinByTank] DiscreteWeaponMagazineControllerNode discreteWeaponMagazineController) {
            CooldownTimerComponent cooldownTimer = discreteWeaponMagazineController.cooldownTimer;

            if (!(cooldownTimer.CooldownTimerSec > 0f) && InputManager.CheckAction(ShotActions.SHOT)) {
                ScheduleEvent<BeforeShotEvent>(discreteWeaponMagazineController);
                ScheduleEvent<ShotPrepareEvent>(discreteWeaponMagazineController);
            }
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankActiveStateComponent tankActiveState;
        }

        public class CommonDiscreteWeaponControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public DiscreteWeaponComponent discreteWeapon;
            public DiscreteWeaponControllerComponent discreteWeaponController;

            public ShootableComponent shootable;
        }

        public class DiscreteWeaponControllerNode : CommonDiscreteWeaponControllerNode {
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class DiscreteWeaponMagazineControllerNode : CommonDiscreteWeaponControllerNode {
            public MagazineReadyStateComponent magazineReadyState;
            public MagazineStorageComponent magazineStorage;
        }
    }
}