using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class StreamWeaponControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void InitializeIdleState(NodeAddedEvent evt, SelfActiveTankNode selfActiveTank, [Context] [JoinByUser] StreamWeaponShootableNode streamWeapon,
            [JoinByUser] ICollection<SelfUserNode> user) {
            Entity entity = streamWeapon.Entity;
            float energy = streamWeapon.weaponEnergy.Energy;

            if (energy <= 0f) {
                SwitchWorkingModeToIdleMode(entity);
            } else if (InputManager.CheckAction(ShotActions.SHOT)) {
                SwitchIdleModeToWorkingMode(entity);
            } else {
                SwitchWorkingModeToIdleMode(entity);
            }
        }

        [OnEventFire]
        public void StartStreamWorkingIfPossible(EarlyUpdateEvent evt, StreamWeaponIdleControllerNode idleWeapon, [JoinSelf] SingleNode<ShootableComponent> shootable,
            [JoinByTank] SelfActiveTankNode selfActiveTank) {
            Entity entity = idleWeapon.Entity;
            float energy = idleWeapon.weaponEnergy.Energy;

            if (!(energy <= 0f) && InputManager.GetActionKeyDown(ShotActions.SHOT)) {
                SwitchIdleModeToWorkingMode(entity);
            }
        }

        [OnEventComplete]
        public void RunWorkingStream(EarlyUpdateEvent evt, StreamWeaponWorkingControllerNode workingWeapon, [JoinByTank] SelfActiveTankNode selfActiveTank) {
            Entity entity = workingWeapon.Entity;
            CooldownTimerComponent cooldownTimer = workingWeapon.cooldownTimer;

            if (workingWeapon.weaponEnergy.Energy <= 0f) {
                SwitchWorkingModeToIdleMode(entity);
            } else if (InputManager.GetActionKeyUp(ShotActions.SHOT)) {
                SwitchWorkingModeToIdleMode(entity);
            } else if (!(cooldownTimer.CooldownTimerSec > 0f) && workingWeapon.Entity.HasComponent<ShootableComponent>()) {
                ScheduleEvent<BeforeShotEvent>(workingWeapon);
                ScheduleEvent<ShotPrepareEvent>(workingWeapon);
            }
        }

        [OnEventFire]
        public void SwitchToIdleWhenTankInactive(NodeRemoveEvent evt, SelfActiveTankNode selfActiveTank, [JoinByTank] StreamWeaponWorkingControllerNode workingWeapon) {
            Entity entity = workingWeapon.Entity;
            SwitchWorkingModeToIdleMode(entity);
        }

        [OnEventFire]
        public void SwitchToIdleWhenRemoveShootable(StreamWeaponResetStateEvent evt, StreamWeaponNode weaponNode) {
            Entity entity = weaponNode.Entity;
            SwitchWorkingModeToIdleMode(entity);
        }

        [OnEventFire]
        public void Clean(NodeRemoveEvent evt, SelfDeadTankNode deadTank, [JoinByTank] StreamWeaponNode idleWeapon) {
            Entity entity = idleWeapon.Entity;
            entity.RemoveComponentIfPresent<StreamWeaponIdleComponent>();
            entity.RemoveComponentIfPresent<StreamWeaponWorkingComponent>();
        }

        public static void SwitchIdleModeToWorkingMode(Entity weapon) {
            weapon.RemoveComponentIfPresent<StreamWeaponIdleComponent>();
            weapon.AddComponentIfAbsent<StreamWeaponWorkingComponent>();
        }

        public static void SwitchWorkingModeToIdleMode(Entity weapon) {
            weapon.RemoveComponentIfPresent<StreamWeaponWorkingComponent>();
            weapon.AddComponentIfAbsent<StreamWeaponIdleComponent>();
        }

        public class StreamWeaponNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public StreamWeaponComponent streamWeapon;
            public StreamWeaponControllerComponent streamWeaponController;

            public TankGroupComponent tankGroup;

            public UserGroupComponent userGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class StreamWeaponShootableNode : StreamWeaponNode {
            public ShootableComponent shootable;
        }

        public class StreamWeaponIdleControllerNode : StreamWeaponNode {
            public StreamWeaponIdleComponent streamWeaponIdle;
        }

        public class StreamWeaponWorkingControllerNode : StreamWeaponNode {
            public StreamWeaponWorkingComponent streamWeaponWorking;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;

            public UserGroupComponent userGroup;
        }

        public class SelfActiveTankNode : SelfTankNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class SelfDeadTankNode : SelfTankNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }
    }
}