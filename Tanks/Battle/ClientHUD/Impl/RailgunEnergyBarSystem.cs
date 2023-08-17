using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class RailgunEnergyBarSystem : ECSSystem {
        [OnEventFire]
        public void Update(TimeUpdateEvent evt, RailgunEnergyNode weaponEnergy, [JoinByTank] ActiveTankNode tank,
            [JoinAll] SingleNode<WeaponBarComponent> weaponBar) =>
            weaponBar.component.ProgressValue = weaponEnergy.weaponEnergy.Energy;

        [OnEventFire]
        public void UpdateCharging(TimeUpdateEvent evt, RailgunChargingEnergyNode weaponEnergy,
            [JoinByTank] ActiveTankNode tank, [JoinAll] SingleNode<WeaponBarComponent> weaponBar) {
            float chargingTime = weaponEnergy.railgunChargingWeapon.ChargingTime;
            weaponBar.component.ProgressValue -= evt.DeltaTime / chargingTime;
        }

        public class RailgunEnergyNode : Node {
            public RailgunChargingWeaponComponent railgunChargingWeapon;
            public RailgunEnergyBarComponent railgunEnergyBar;

            public ReadyRailgunChargingWeaponComponent readyRailgunChargingWeapon;

            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class RailgunChargingEnergyNode : Node {
            public RailgunChargingStateComponent railgunChargingState;

            public RailgunChargingWeaponComponent railgunChargingWeapon;
            public RailgunEnergyBarComponent railgunEnergyBar;

            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class ActiveTankNode : Node {
            public SelfTankComponent selfTank;
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}