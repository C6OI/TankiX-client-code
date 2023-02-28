using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class LocalHammerMagazineSystem : ECSSystem {
        [OnEventFire]
        public void InitLocalMagazineStorage(NodeAddedEvent evt, InitialHammerMagazineNode weapon) {
            int currentCartridgeCount = weapon.magazineStorage.CurrentCartridgeCount;
            weapon.Entity.AddComponent(new MagazineLocalStorageComponent(currentCartridgeCount));
        }

        [OnEventFire]
        public void ResetLocalMagazineStorage(NodeAddedEvent evt, LocalHammerMagazineNode weapon, [JoinByTank] [Context] TankIncarnationNode tank) {
            weapon.magazineLocalStorage.CurrentCartridgeCount = weapon.magazineWeapon.MaxCartridgeCount;
        }

        [OnEventFire]
        public void ResetLocalMagazineStorage(ExecuteEnergyInjectionEvent e, LocalHammerMagazineNode weapon) {
            weapon.magazineLocalStorage.CurrentCartridgeCount = weapon.magazineWeapon.MaxCartridgeCount;
        }

        [OnEventComplete]
        public void UpdateLocalMagazineStorage(BaseShotEvent evt, LocalHammerMagazineNode weapon) {
            int maxCartridgeCount = weapon.magazineWeapon.MaxCartridgeCount;

            if (weapon.magazineLocalStorage.CurrentCartridgeCount == 1) {
                weapon.magazineLocalStorage.CurrentCartridgeCount = maxCartridgeCount;
            } else {
                weapon.magazineLocalStorage.CurrentCartridgeCount--;
            }
        }

        public class InitialHammerMagazineNode : Node {
            public HammerComponent hammer;
            public MagazineStorageComponent magazineStorage;

            public MagazineWeaponComponent magazineWeapon;
        }

        public class LocalHammerMagazineNode : Node {
            public MagazineLocalStorageComponent magazineLocalStorage;

            public MagazineWeaponComponent magazineWeapon;

            public TankGroupComponent tankGroup;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }
    }
}