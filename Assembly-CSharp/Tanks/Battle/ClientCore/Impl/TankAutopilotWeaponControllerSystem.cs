using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankAutopilotWeaponControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void StartShotIfPossible(EarlyUpdateEvent evt, DiscreteWeaponControllerNode discreteWeaponEnergyController, [JoinByTank] AutopilotTankNode autopilotTank) {
            float unloadEnergyPerShot = discreteWeaponEnergyController.discreteWeaponEnergy.UnloadEnergyPerShot;
            float energy = discreteWeaponEnergyController.weaponEnergy.Energy;
            CooldownTimerComponent cooldownTimer = discreteWeaponEnergyController.cooldownTimer;

            if (autopilotTank.autopilotWeaponController.Fire && !(energy < unloadEnergyPerShot) && !(cooldownTimer.CooldownTimerSec > 0f)) {
                ScheduleEvent<BeforeShotEvent>(discreteWeaponEnergyController);
                ScheduleEvent<ShotPrepareEvent>(discreteWeaponEnergyController);
                ScheduleEvent<PostShotEvent>(discreteWeaponEnergyController);
            }
        }

        [OnEventFire]
        public void StartShotIfPossible(EarlyUpdateEvent evt, DiscreteWeaponMagazineControllerNode discreteWeaponMagazineController, [JoinByTank] AutopilotTankNode autopilotTank) {
            if (autopilotTank.autopilotWeaponController.Fire) {
                CooldownTimerComponent cooldownTimer = discreteWeaponMagazineController.cooldownTimer;

                if (!(cooldownTimer.CooldownTimerSec > 0f)) {
                    ScheduleEvent<BeforeShotEvent>(discreteWeaponMagazineController);
                    ScheduleEvent<ShotPrepareEvent>(discreteWeaponMagazineController);
                }
            }
        }

        public class AutopilotTankNode : Node {
            public AutopilotWeaponControllerComponent autopilotWeaponController;

            public TankActiveStateComponent tankActiveState;

            public TankAutopilotComponent tankAutopilot;
            public TankSyncComponent tankSync;
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