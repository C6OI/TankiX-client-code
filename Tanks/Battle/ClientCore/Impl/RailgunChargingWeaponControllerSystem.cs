using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class RailgunChargingWeaponControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void StartShotIfPossible(EarlyUpdateEvent evt,
            ReadyRailgunChargingWeaponControllerNode chargingWeaponController, [JoinByTank] ActiveTankNode selfActiveTank) {
            float unloadEnergyPerShot = chargingWeaponController.discreteWeaponEnergy.UnloadEnergyPerShot;
            float energy = chargingWeaponController.weaponEnergy.Energy;
            CooldownTimerComponent cooldownTimer = chargingWeaponController.cooldownTimer;

            if (!(energy < unloadEnergyPerShot) &&
                !(cooldownTimer.CooldownTimerSec > 0f) &&
                InputManager.CheckAction(ShotActions.SHOT)) {
                ScheduleEvent<SelfRailgunChargingShotEvent>(chargingWeaponController);
            }
        }

        [OnEventFire]
        public void SendShotPrepare(RailgunDelayedShotPrepareEvent evt,
            CompleteChargingWeaponControllerNode chargingWeaponNode, [JoinByTank] ActiveTankNode selfActiveTank) {
            Entity entity = chargingWeaponNode.Entity;
            entity.AddComponent<ReadyRailgunChargingWeaponComponent>();
            entity.RemoveComponent<RailgunChargingStateComponent>();
            ScheduleEvent<BeforeShotEvent>(entity);
            ScheduleEvent<ShotPrepareEvent>(entity);
        }

        [OnEventFire]
        public void MakeChargingAndScheduleShot(SelfRailgunChargingShotEvent evt,
            ReadyRailgunChargingWeaponControllerNode chargingWeaponController) {
            Entity entity = chargingWeaponController.Entity;
            entity.RemoveComponent<ReadyRailgunChargingWeaponComponent>();
            entity.AddComponent<RailgunChargingStateComponent>();
            float chargingTime = chargingWeaponController.railgunChargingWeapon.ChargingTime;
            EventBuilder eventBuilder = NewEvent<RailgunDelayedShotPrepareEvent>();
            eventBuilder.Attach(chargingWeaponController);
            eventBuilder.ScheduleDelayed(chargingTime);
        }

        public class ReadyRailgunChargingWeaponControllerNode : Node {
            public ChargingWeaponControllerComponent chargingWeaponController;

            public CooldownTimerComponent cooldownTimer;

            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;

            public RailgunChargingWeaponComponent railgunChargingWeapon;

            public ReadyRailgunChargingWeaponComponent readyRailgunChargingWeapon;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class CompleteChargingWeaponControllerNode : Node {
            public ChargingWeaponControllerComponent chargingWeaponController;

            public RailgunChargingStateComponent railgunChargingState;

            public RailgunChargingWeaponComponent railgunChargingWeapon;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;
        }
    }
}