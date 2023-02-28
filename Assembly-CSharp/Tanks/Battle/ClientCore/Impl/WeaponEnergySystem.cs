using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponEnergySystem : ECSSystem {
        [OnEventFire]
        public void InitWeaponEnergyStates(NodeAddedEvent evt, WeaponEnergyNode weapon, [Context] [JoinByTank] ActiveTankNode activeTank) {
            WeaponEnergyESMComponent weaponEnergyESMComponent = new();
            EntityStateMachine esm = weaponEnergyESMComponent.Esm;
            weapon.weaponEnergy.Energy = 1f;
            esm.AddState<WeaponEnergyStates.WeaponEnergyFullState>();
            esm.AddState<WeaponEnergyStates.WeaponEnergyReloadingState>();
            esm.AddState<WeaponEnergyStates.WeaponEnergyUnloadingState>();
            weapon.Entity.AddComponent(weaponEnergyESMComponent);
        }

        [OnEventFire]
        public void DestroyWeaponEnergyStates(NodeRemoveEvent evt, ActiveTankNode node, [JoinByTank] WeaponEnergyWithESMNode weapon) {
            weapon.Entity.RemoveComponent<WeaponEnergyESMComponent>();
        }

        public class WeaponEnergyNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class WeaponEnergyWithESMNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }
    }
}