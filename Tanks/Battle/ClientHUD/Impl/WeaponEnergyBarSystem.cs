using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class WeaponEnergyBarSystem : ECSSystem {
        [OnEventFire]
        public void Update(UpdateEvent evt, WeaponEnergyNode weaponEnergy, [JoinByTank] ActiveTankNode tank,
            [JoinAll] SingleNode<WeaponBarComponent> weaponBar) =>
            weaponBar.component.ProgressValue = weaponEnergy.weaponEnergy.Energy;

        [OnEventFire]
        public void InitWeaponBar(NodeAddedEvent e, SingleNode<WeaponBarComponent> weaponBar) => FillWeaponBar(weaponBar);

        [OnEventFire]
        public void FillWeaponBar(NodeAddedEvent e, SelfTankSpawnNode tank,
            [JoinAll] SingleNode<WeaponBarComponent> weaponBar) => FillWeaponBar(weaponBar);

        static void FillWeaponBar(SingleNode<WeaponBarComponent> weaponBar) => weaponBar.component.ProgressValue = 1f;

        public class WeaponEnergyNode : Node {
            public EnergyBarComponent energyBar;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class ActiveTankNode : Node {
            public SelfTankComponent selfTank;
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }

        public class SelfTankSpawnNode : Node {
            public SelfTankComponent selfTank;

            public TankSpawnStateComponent tankSpawnState;
        }
    }
}