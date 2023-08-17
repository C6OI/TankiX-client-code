using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class DiscreteWeaponControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void StartShotIfPossible(EarlyUpdateEvent evt, DiscreteWeaponControllerNode discreteWeaponEnergyController,
            [JoinByTank] SingleNode<TankActiveStateComponent> activeTank) {
            float unloadEnergyPerShot = discreteWeaponEnergyController.discreteWeaponEnergy.UnloadEnergyPerShot;
            float energy = discreteWeaponEnergyController.weaponEnergy.Energy;
            CooldownTimerComponent cooldownTimer = discreteWeaponEnergyController.cooldownTimer;

            if (!(energy < unloadEnergyPerShot) &&
                !(cooldownTimer.CooldownTimerSec > 0f) &&
                InputManager.CheckAction(ShotActions.SHOT)) {
                ScheduleEvent<BeforeShotEvent>(discreteWeaponEnergyController);
                ScheduleEvent<ShotPrepareEvent>(discreteWeaponEnergyController);
            }
        }

        [OnEventFire]
        public void StartShotIfPossible(EarlyUpdateEvent evt,
            DiscreteWeaponMagazineControllerNode discreteWeaponMagazineController,
            [JoinByTank] SingleNode<TankActiveStateComponent> activeTank) {
            CooldownTimerComponent cooldownTimer = discreteWeaponMagazineController.cooldownTimer;

            if (!(cooldownTimer.CooldownTimerSec > 0f) && InputManager.CheckAction(ShotActions.SHOT)) {
                ScheduleEvent<BeforeShotEvent>(discreteWeaponMagazineController);
                ScheduleEvent<ShotPrepareEvent>(discreteWeaponMagazineController);
            }
        }

        public class DiscreteWeaponControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public DiscreteWeaponComponent discreteWeapon;
            public DiscreteWeaponControllerComponent discreteWeaponController;

            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class DiscreteWeaponMagazineControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public DiscreteWeaponComponent discreteWeapon;
            public DiscreteWeaponControllerComponent discreteWeaponController;

            public MagazineReadyStateComponent magazineReadyState;

            public MagazineStorageComponent magazineStorage;
        }
    }
}