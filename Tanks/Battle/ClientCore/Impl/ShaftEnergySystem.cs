using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftEnergySystem : ECSSystem {
        [OnEventFire]
        public void UpdateExhaustedWorkingEnergy(TimeUpdateEvent evt, ShaftWorkingEnergyNode weapon) {
            weapon.shaftAimingWorkingState.ExhaustedEnergy += weapon.shaftEnergy.UnloadAimingEnergyPerSec * evt.DeltaTime;

            weapon.shaftAimingWorkingState.ExhaustedEnergy = Mathf.Clamp(weapon.shaftAimingWorkingState.ExhaustedEnergy,
                0f,
                weapon.shaftAimingWorkingState.InitialEnergy);
        }

        [OnEventFire]
        public void ReloadEnergy(TimeUpdateEvent evt, ShaftEnergyNode weapon,
            [JoinByTank] SingleNode<WeaponEnergyReloadingStateComponent> reloading, [JoinByTank] ActiveTankNode tank) =>
            ReloadEnergy(evt, weapon);

        [OnEventFire]
        public void UnloadEnergy(TimeUpdateEvent evt, ShaftEnergyNode weapon,
            [JoinByTank] SingleNode<WeaponEnergyUnloadingStateComponent> unloading, [JoinByTank] ActiveTankNode tank) =>
            UnloadEnergy(evt, weapon);

        [OnEventFire]
        public void StartReloading(NodeAddedEvent evt, ShaftEnergyNode weapon,
            [JoinByTank] [Context] SingleNode<ShaftIdleStateComponent> state, [JoinByTank] [Context] ActiveTankNode tank) =>
            weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyReloadingState>();

        [OnEventFire]
        public void StartReloading(NodeAddedEvent evt, ShaftEnergyNode weapon,
            [JoinByTank] [Context] SingleNode<ShaftWaitingStateComponent> state,
            [Context] [JoinByTank] ActiveTankNode tank) =>
            weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyReloadingState>();

        [OnEventFire]
        public void StartReloading(NodeAddedEvent evt, ShaftEnergyNode weapon,
            [Context] [JoinByTank] SingleNode<ShaftAimingWorkFinishStateComponent> state,
            [Context] [JoinByTank] ActiveTankNode tank) =>
            weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyReloadingState>();

        [OnEventFire]
        public void StartUnloading(NodeAddedEvent evt, ShaftEnergyNode weapon,
            [JoinByTank] [Context] SingleNode<ShaftAimingWorkActivationStateComponent> state,
            [Context] [JoinByTank] ActiveTankNode tank) =>
            weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyUnloadingState>();

        [OnEventFire]
        public void StartUnloading(NodeAddedEvent evt, ShaftEnergyNode weapon,
            [Context] [JoinByTank] SingleNode<ShaftAimingWorkingStateComponent> state,
            [Context] [JoinByTank] ActiveTankNode tank) =>
            weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyUnloadingState>();

        [OnEventFire]
        public void UnloadEnergyPerQuickShot(BaseShotEvent evt, ShaftEnergyNode weapon,
            [JoinByTank] SingleNode<ShaftIdleStateComponent> state) => UnloadEnergyByQuickShot(weapon);

        [OnEventFire]
        public void UnloadEnergyPerAimingShot(BaseShotEvent evt, ShaftEnergyNode weapon,
            [JoinByTank] SingleNode<ShaftAimingWorkFinishStateComponent> state) => UnloadEnergyByAimingShot(weapon);

        void UnloadEnergyByQuickShot(ShaftEnergyNode weapon) {
            float deltaEnergy = 0f - weapon.shaftEnergy.UnloadEnergyPerQuickShot;
            ApplyDeltaEnergy(deltaEnergy, weapon);
        }

        void UnloadEnergyByAimingShot(ShaftEnergyNode weapon) {
            weapon.weaponEnergy.Energy = Mathf.Min(weapon.weaponEnergy.Energy,
                1f - weapon.shaftEnergy.PossibleUnloadEnergyPerAimingShot);

            weapon.weaponEnergy.Energy = Mathf.Clamp01(weapon.weaponEnergy.Energy);
        }

        void ReloadEnergy(TimeUpdateEvent evt, ShaftEnergyNode weapon) {
            float num = weapon.shaftEnergy.ReloadEnergyPerSec * evt.DeltaTime;
            weapon.weaponEnergy.Energy += num;

            if (weapon.weaponEnergy.Energy >= 1f) {
                weapon.weaponEnergy.Energy = 1f;
                weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyFullState>();
            }
        }

        void UnloadEnergy(TimeUpdateEvent evt, ShaftEnergyNode weapon) {
            float deltaEnergy = (0f - weapon.shaftEnergy.UnloadAimingEnergyPerSec) * evt.DeltaTime;
            ApplyDeltaEnergy(deltaEnergy, weapon);
        }

        void ApplyDeltaEnergy(float deltaEnergy, ShaftEnergyNode weapon) {
            weapon.weaponEnergy.Energy += deltaEnergy;
            weapon.weaponEnergy.Energy = Mathf.Clamp01(weapon.weaponEnergy.Energy);
        }

        public class ShaftEnergyNode : Node {
            public DiscreteWeaponComponent discreteWeapon;
            public ShaftEnergyComponent shaftEnergy;

            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponEnergyESMComponent weaponEnergyEsm;
        }

        public class ShaftWorkingEnergyNode : Node {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public ShaftEnergyComponent shaftEnergy;
            public ShaftStateControllerComponent shaftStateController;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}