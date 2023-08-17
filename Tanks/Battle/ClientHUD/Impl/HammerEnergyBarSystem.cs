using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HammerEnergyBarSystem : ECSSystem {
        [OnEventFire]
        public void UpdateOnTrigger(BaseShotEvent evt, HammerReadyEnergyNode hammerEnergy,
            [JoinByTank] SingleNode<SelfTankComponent> selfNode, [JoinAll] SingleNode<WeaponBarComponent> weaponBar) {
            float num = hammerEnergy.magazineWeapon.MaxCartridgeCount;
            float num2 = hammerEnergy.magazineLocalStorage.CurrentCartridgeCount - 1;
            weaponBar.component.ProgressValue = num2 / num;
        }

        [OnEventFire]
        public void Reload(NodeAddedEvent evt, HammerReloadEnergyNode hammerEnergy,
            [JoinByTank] [Context] SelfTankNode selfTankNode, [JoinAll] SingleNode<WeaponBarComponent> weaponBar) =>
            weaponBar.component.ProgressValue = 0f;

        [OnEventFire]
        public void SetReady(NodeAddedEvent evt, HammerReadyEnergyNode hammerEnergy,
            [Context] [JoinByTank] SelfTankNode selfTankNode, [JoinAll] SingleNode<WeaponBarComponent> weaponBar) =>
            weaponBar.component.ProgressValue = 1f;

        [OnEventFire]
        public void UpdateReload(TimeUpdateEvent evt, HammerReloadEnergyNode hammerEnergy,
            [JoinByTank] SingleNode<SelfTankComponent> selfNode, [JoinAll] SingleNode<WeaponBarComponent> weaponBar) =>
            weaponBar.component.ProgressValue += evt.DeltaTime / hammerEnergy.magazineWeapon.ReloadMagazineTimePerSec;

        public class HammerEnergyNode : Node {
            public HammerComponent hammer;

            public HammerEnergyBarComponent hammerEnergyBar;

            public MagazineStorageComponent magazineStorage;

            public MagazineWeaponComponent magazineWeapon;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class HammerReadyEnergyNode : Node {
            public HammerComponent hammer;

            public HammerEnergyBarComponent hammerEnergyBar;

            public MagazineLocalStorageComponent magazineLocalStorage;

            public MagazineReadyStateComponent magazineReadyState;

            public MagazineStorageComponent magazineStorage;

            public MagazineWeaponComponent magazineWeapon;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class HammerReloadEnergyNode : Node {
            public HammerComponent hammer;

            public HammerEnergyBarComponent hammerEnergyBar;

            public MagazineReloadStateComponent magazineReloadState;

            public MagazineStorageComponent magazineStorage;

            public MagazineWeaponComponent magazineWeapon;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;

            public TankGroupComponent tankGroup;
        }
    }
}