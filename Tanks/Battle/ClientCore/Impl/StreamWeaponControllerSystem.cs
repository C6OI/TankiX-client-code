using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class StreamWeaponControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void InitializeIdleState(NodeAddedEvent evt, ActiveTankNode selfActiveTank,
            [Context] [JoinByUser] StreamWeaponNode streamWeapon, [JoinByUser] ICollection<SelfUserNode> user) {
            Entity entity = streamWeapon.Entity;

            if (InputManager.CheckAction(ShotActions.SHOT)) {
                entity.AddComponent<StreamWeaponWorkingComponent>();
            } else {
                entity.AddComponent<StreamWeaponIdleComponent>();
            }
        }

        [OnEventFire]
        public void StartStreamWorkingIfPossible(EarlyUpdateEvent evt, StreamWeaponIdleControllerNode idleWeapon,
            [JoinByTank] ActiveTankNode selfActiveTank) {
            Entity entity = idleWeapon.Entity;
            float energy = idleWeapon.weaponEnergy.Energy;

            if (!(energy <= 0f) && InputManager.GetActionKeyDown(ShotActions.SHOT)) {
                SwitchIdleModeToWorkingMode(entity);
            }
        }

        [OnEventComplete]
        public void RunWorkingStream(EarlyUpdateEvent evt, StreamWeaponWorkingControllerNode workingWeapon,
            [JoinByTank] ActiveTankNode selfActiveTank) {
            Entity entity = workingWeapon.Entity;
            CooldownTimerComponent cooldownTimer = workingWeapon.cooldownTimer;

            if (workingWeapon.weaponEnergy.Energy <= 0f) {
                SwitchWorkingModeToIdleMode(entity);
            } else if (InputManager.GetActionKeyUp(ShotActions.SHOT)) {
                SwitchWorkingModeToIdleMode(entity);
            } else if (!(cooldownTimer.CooldownTimerSec > 0f)) {
                ScheduleEvent<BeforeShotEvent>(workingWeapon);
                ScheduleEvent<ShotPrepareEvent>(workingWeapon);
            }
        }

        [OnEventFire]
        public void SwitchToIdleWhenTankInactive(NodeRemoveEvent evt, ActiveTankNode selfActiveTank,
            [JoinByTank] StreamWeaponWorkingControllerNode workingWeapon) {
            Entity entity = workingWeapon.Entity;
            SwitchWorkingModeToIdleMode(entity);
        }

        void SwitchIdleModeToWorkingMode(Entity weapon) {
            weapon.RemoveComponent<StreamWeaponIdleComponent>();
            weapon.AddComponent<StreamWeaponWorkingComponent>();
        }

        void SwitchWorkingModeToIdleMode(Entity weapon) {
            weapon.RemoveComponent<StreamWeaponWorkingComponent>();
            weapon.AddComponent<StreamWeaponIdleComponent>();
        }

        public class StreamWeaponNode : Node {
            public StreamWeaponComponent streamWeapon;
            public TankGroupComponent tankGroup;
        }

        public class StreamWeaponIdleControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public StreamWeaponComponent streamWeapon;
            public StreamWeaponControllerComponent streamWeaponController;

            public StreamWeaponIdleComponent streamWeaponIdle;

            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class StreamWeaponWorkingControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public StreamWeaponComponent streamWeapon;
            public StreamWeaponControllerComponent streamWeaponController;

            public StreamWeaponWorkingComponent streamWeaponWorking;

            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class ActiveTankNode : Node {
            public SelfTankComponent selfTank;
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;
        }
    }
}