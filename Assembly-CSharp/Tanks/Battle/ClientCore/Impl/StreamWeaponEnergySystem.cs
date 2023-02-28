using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class StreamWeaponEnergySystem : ECSSystem {
        [OnEventFire]
        public void StartReloading(NodeAddedEvent evt, StreamWeaponIdleEnergyNode weapon, [Context] [JoinByTank] ActiveTankNode activeTank) {
            if (weapon.weaponEnergy.Energy == 1f) {
                weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyFullState>();
            } else {
                weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyReloadingState>();
            }
        }

        [OnEventFire]
        public void StartUnloading(NodeAddedEvent evt, StreamWeaponWorkingEnergyNode weapon) {
            weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyUnloadingState>();
        }

        [OnEventFire]
        public void UnloadEnergy(TimeUpdateEvent evt, StreamWeaponUnloadingEnergyNode weapon) {
            float num = weapon.streamWeaponEnergy.UnloadEnergyPerSec * evt.DeltaTime;
            weapon.weaponEnergy.Energy -= num;
            weapon.weaponEnergy.Energy = Mathf.Clamp(weapon.weaponEnergy.Energy, 0f, 1f);
        }

        [OnEventFire]
        public void ReloadEnergy(TimeUpdateEvent evt, StreamWeaponReloadingEnergyNode weapon, [JoinByTank] ActiveTankNode activeTank) {
            float num = weapon.streamWeaponEnergy.ReloadEnergyPerSec * evt.DeltaTime;
            weapon.weaponEnergy.Energy += num;

            if (weapon.weaponEnergy.Energy >= 1f) {
                weapon.weaponEnergy.Energy = 1f;
                weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyFullState>();
            }
        }

        public class StreamWeaponWorkingEnergyNode : Node {
            public StreamWeaponComponent streamWeapon;

            public StreamWeaponWorkingComponent streamWeaponWorking;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponEnergyESMComponent weaponEnergyEsm;
        }

        public class StreamWeaponIdleEnergyNode : Node {
            public StreamWeaponComponent streamWeapon;

            public StreamWeaponIdleComponent streamWeaponIdle;
            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponEnergyESMComponent weaponEnergyEsm;
        }

        public class StreamWeaponUnloadingEnergyNode : Node {
            public StreamWeaponComponent streamWeapon;

            public StreamWeaponEnergyComponent streamWeaponEnergy;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponEnergyESMComponent weaponEnergyEsm;

            public WeaponEnergyUnloadingStateComponent weaponEnergyUnloadingState;
        }

        public class StreamWeaponReloadingEnergyNode : Node {
            public StreamWeaponComponent streamWeapon;

            public StreamWeaponEnergyComponent streamWeaponEnergy;
            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponEnergyESMComponent weaponEnergyEsm;

            public WeaponEnergyReloadingStateComponent weaponEnergyReloadingState;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}