using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class VulcanEnergyBarSystem : ECSSystem {
        [OnEventFire]
        public void DrawEnergyWorking(UpdateEvent e, VulcanWeaponWorkingControllerNode vulcan,
            [JoinAll] SingleNode<WeaponBarComponent> weaponBar) {
            float num = Date.Now - vulcan.vulcanShooting.StartShootingTime;
            float temperatureHittingTime = vulcan.vulcanWeapon.TemperatureHittingTime;
            float num2 = temperatureHittingTime - num;

            if (num2 < 0f) {
                num2 = 0f;
            }

            weaponBar.component.ProgressValue = num2 / temperatureHittingTime;
        }

        [OnEventFire]
        public void DrawEnergySlowDown(UpdateEvent e, VulcanWeaponSlowDownControllerNode vulcan,
            [JoinAll] SingleNode<WeaponBarComponent> weaponBar) =>
            weaponBar.component.ProgressValue = 1f - vulcan.vulcanWeaponState.State;

        [OnEventFire]
        public void DrawEnergyIdle(UpdateEvent e, VulcanWeaponIdleControllerNode vulcan,
            [JoinAll] SingleNode<WeaponBarComponent> weaponBar) => weaponBar.component.ProgressValue = 1f;

        [OnEventFire]
        public void DrawEnergySpeedUp(UpdateEvent e, VulcanWeaponSpeedUpControllerNode vulcan,
            [JoinAll] SingleNode<WeaponBarComponent> weaponBar) => weaponBar.component.ProgressValue = 1f;

        public class VulcanWeaponWorkingControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public TankGroupComponent tankGroup;

            public VulcanEnergyBarComponent vulcanEnergyBar;

            public VulcanShootingComponent vulcanShooting;
            public VulcanWeaponComponent vulcanWeapon;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanWeaponSlowDownControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public TankGroupComponent tankGroup;

            public VulcanEnergyBarComponent vulcanEnergyBar;

            public VulcanSlowDownComponent vulcanSlowDown;
            public VulcanWeaponComponent vulcanWeapon;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanWeaponIdleControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public TankGroupComponent tankGroup;

            public VulcanEnergyBarComponent vulcanEnergyBar;

            public VulcanIdleComponent vulcanIdle;
            public VulcanWeaponComponent vulcanWeapon;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanWeaponSpeedUpControllerNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public TankGroupComponent tankGroup;

            public VulcanEnergyBarComponent vulcanEnergyBar;

            public VulcanSpeedUpComponent vulcanSpeedUp;
            public VulcanWeaponComponent vulcanWeapon;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }
    }
}