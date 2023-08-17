using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class WeaponEnergyStates {
        public class WeaponEnergyFullState : Node {
            public WeaponEnergyFullStateComponent weaponEnergyFullState;
        }

        public class WeaponEnergyReloadingState : Node {
            public WeaponEnergyReloadingStateComponent weaponEnergyReloadingState;
        }

        public class WeaponEnergyUnloadingState : Node {
            public WeaponEnergyUnloadingStateComponent weaponEnergyUnloadingState;
        }
    }
}