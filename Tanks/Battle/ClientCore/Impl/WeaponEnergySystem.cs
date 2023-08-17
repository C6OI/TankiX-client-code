using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponEnergySystem : ECSSystem {
        [OnEventFire]
        public void InitWeaponEnergyStates(NodeAddedEvent evt, WeaponEnergyNode weapon,
            [JoinByTank] [Context] ActiveTankNode activeTank) {
            WeaponEnergyESMComponent weaponEnergyESMComponent = new();
            EntityStateMachine esm = weaponEnergyESMComponent.Esm;
            esm.AddState<WeaponEnergyStates.WeaponEnergyFullState>();
            esm.AddState<WeaponEnergyStates.WeaponEnergyReloadingState>();
            esm.AddState<WeaponEnergyStates.WeaponEnergyUnloadingState>();
            weapon.Entity.AddComponent(weaponEnergyESMComponent);
        }

        public class WeaponEnergyNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }
    }
}