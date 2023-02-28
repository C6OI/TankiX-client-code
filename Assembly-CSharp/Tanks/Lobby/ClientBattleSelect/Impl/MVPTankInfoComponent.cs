using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPTankInfoComponent : MonoBehaviour {
        [SerializeField] TankPartInfoComponent hull;

        [SerializeField] TankPartInfoComponent turret;

        ModuleUpgradablePowerConfigComponent moduleConfig;

        public void Set(UserResult mvp) {
            hull.Set(mvp.HullId, mvp.Modules, moduleConfig);
            turret.Set(mvp.WeaponId, mvp.Modules, moduleConfig);
        }

        internal void SetModuleConfig(ModuleUpgradablePowerConfigComponent moduleConfig) {
            this.moduleConfig = moduleConfig;
        }
    }
}