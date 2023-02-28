using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftStateControllerSystem : ECSSystem {
        readonly HashSet<Type> weaponStates;

        public ShaftStateControllerSystem() {
            weaponStates = new HashSet<Type>();
            weaponStates.Add(typeof(ShaftIdleStateComponent));
            weaponStates.Add(typeof(ShaftWaitingStateComponent));
            weaponStates.Add(typeof(ShaftAimingWorkActivationStateComponent));
            weaponStates.Add(typeof(ShaftAimingWorkingStateComponent));
            weaponStates.Add(typeof(ShaftAimingWorkFinishStateComponent));
        }

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void InitIdleState(NodeAddedEvent evt, ShaftWeaponNode weapon) {
            StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, weaponStates);
        }

        [OnEventFire]
        public void InitWaitingStateOnInput(TimeUpdateEvent evt, ShaftIdleWeaponControllerNode weapon, [JoinByTank] SelfActiveTankNode activeTank) {
            StartWaitingStateIfPossible(weapon.Entity);
        }

        [OnEventFire]
        public void CheckWaitingState(TimeUpdateEvent evt, ShaftWaitingWeaponControllerNode weapon) {
            if (InputManager.CheckAction(ShotActions.SHOT)) {
                StartWorkActivationStateIfPossible(weapon, evt.DeltaTime);
            } else {
                StartQuickShotIfPossible(weapon);
            }
        }

        [OnEventFire]
        public void CheckWorkActivationState(TimeUpdateEvent evt, ShaftAimingWorkActivationWeaponControllerNode weapon) {
            if (CheckHandleWeaponIntersectionStatus(weapon.Entity)) {
                return;
            }

            if (!InputManager.CheckAction(ShotActions.SHOT)) {
                MakeQuickShot(weapon.Entity);
                return;
            }

            float activationTimer = weapon.shaftAimingWorkActivationState.ActivationTimer;
            float activationToWorkingTransitionTimeSec = weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec;

            if (activationTimer < activationToWorkingTransitionTimeSec) {
                weapon.shaftAimingWorkActivationState.ActivationTimer += evt.DeltaTime;
                return;
            }

            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);
            ShaftAimingWorkingStateComponent shaftAimingWorkingStateComponent = new();
            shaftAimingWorkingStateComponent.InitialEnergy = weapon.weaponEnergy.Energy;
            shaftAimingWorkingStateComponent.WorkingDirection = muzzleLogicAccessor.GetFireDirectionWorld();
            ShaftAimingWorkingStateComponent component = shaftAimingWorkingStateComponent;
            StateUtils.SwitchEntityState(weapon.Entity, component, weaponStates);
        }

        [OnEventFire]
        public void CheckWorkingState(EarlyUpdateEvent evt, ShaftAimingWorkingWeaponControllerNode weapon) {
            if (!CheckHandleWeaponIntersectionStatus(weapon.Entity) && !InputManager.CheckAction(ShotActions.SHOT)) {
                MakeAimingShot(weapon.Entity, weapon.shaftAimingWorkingState.WorkingDirection);
            }
        }

        [OnEventFire]
        public void CheckWorkFinishState(TimeUpdateEvent evt, ShaftAimingWorkFinishWeaponControllerNode weapon) {
            float finishTimer = weapon.shaftAimingWorkFinishState.FinishTimer;
            float finishToIdleTransitionTimeSec = weapon.shaftStateConfig.FinishToIdleTransitionTimeSec;

            if (finishTimer < finishToIdleTransitionTimeSec) {
                weapon.shaftAimingWorkFinishState.FinishTimer += evt.DeltaTime;
            } else {
                StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, weaponStates);
            }
        }

        [OnEventFire]
        public void CheckWeaponStateOnInactiveTank(NodeRemoveEvent evt, SelfActiveTankNode tank, [JoinByTank] ShaftWeaponNode weapon) {
            StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, weaponStates);
        }

        void StartWorkActivationStateIfPossible(ShaftWaitingWeaponControllerNode weapon, float dt) {
            if (weapon.Entity.HasComponent<WeaponUndergroundComponent>()) {
                return;
            }

            float waitingTimer = weapon.shaftWaitingState.WaitingTimer;
            float waitingToActivationTransitionTimeSec = weapon.shaftStateConfig.WaitingToActivationTransitionTimeSec;

            if (waitingTimer < waitingToActivationTransitionTimeSec) {
                weapon.shaftWaitingState.WaitingTimer += dt;
                return;
            }

            float energy = weapon.weaponEnergy.Energy;

            if (!(energy < 1f)) {
                StateUtils.SwitchEntityState<ShaftAimingWorkActivationStateComponent>(weapon.Entity, weaponStates);
            }
        }

        void StartWaitingStateIfPossible(Entity weapon) {
            if (InputManager.CheckAction(ShotActions.SHOT)) {
                StateUtils.SwitchEntityState<ShaftWaitingStateComponent>(weapon, weaponStates);
            }
        }

        void StartQuickShotIfPossible(ShaftWaitingWeaponControllerNode weapon) {
            float unloadEnergyPerQuickShot = weapon.shaftEnergy.UnloadEnergyPerQuickShot;
            float energy = weapon.weaponEnergy.Energy;
            CooldownTimerComponent cooldownTimer = weapon.cooldownTimer;

            if (energy < unloadEnergyPerQuickShot) {
                StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, weaponStates);
            } else if (cooldownTimer.CooldownTimerSec > 0f) {
                StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, weaponStates);
            } else {
                MakeQuickShot(weapon.Entity);
            }
        }

        bool CheckHandleWeaponIntersectionStatus(Entity weapon) {
            bool flag = weapon.HasComponent<WeaponUndergroundComponent>();

            if (flag) {
                StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon, weaponStates);
            }

            return flag;
        }

        void MakeQuickShot(Entity weapon) {
            StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon, weaponStates);

            if (weapon.HasComponent<ShootableComponent>()) {
                ScheduleEvent<BeforeShotEvent>(weapon);
                ScheduleEvent<ShotPrepareEvent>(weapon);
            }
        }

        void MakeAimingShot(Entity weapon, Vector3 workingDir) {
            StateUtils.SwitchEntityState<ShaftAimingWorkFinishStateComponent>(weapon, weaponStates);

            if (weapon.HasComponent<ShootableComponent>()) {
                ScheduleEvent<BeforeShotEvent>(weapon);
                ScheduleEvent(new ShaftAimingShotPrepareEvent(workingDir), weapon);
            }
        }

        public class ShaftWeaponNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public DiscreteWeaponComponent discreteWeapon;

            public ShaftStateConfigComponent shaftStateConfig;

            public ShaftStateControllerComponent shaftStateController;
            public WeaponEnergyComponent weaponEnergy;
        }

        public class ShaftIdleWeaponControllerNode : ShaftWeaponNode {
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class ShaftWaitingWeaponControllerNode : ShaftWeaponNode {
            public ShaftEnergyComponent shaftEnergy;
            public ShaftWaitingStateComponent shaftWaitingState;
        }

        public class ShaftAimingWorkActivationWeaponControllerNode : ShaftWeaponNode {
            public MuzzlePointComponent muzzlePoint;
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;

            public WeaponInstanceComponent weaponInstance;
        }

        public class ShaftAimingWorkingWeaponControllerNode : ShaftWeaponNode {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }

        public class ShaftAimingWorkFinishWeaponControllerNode : ShaftWeaponNode {
            public ShaftAimingWorkFinishStateComponent shaftAimingWorkFinishState;
        }

        public class SelfActiveTankNode : Node {
            public SelfTankComponent selfTank;
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}