using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPScreenUIComponent : BehaviourComponent {
        public static int ShowCounter;

        [SerializeField] MVPUserMainInfoComponent userInfo;

        [SerializeField] MVPMainStatComponent mainStat;

        [SerializeField] MVPOtherStatComponent otherStat;

        [SerializeField] MVPTankInfoComponent tankInfo;

        [SerializeField] MVPModulesInfoComponent modulesInfo;

        [SerializeField] TimerWithAction continueTimer;

        [SerializeField] float timeIfMvpIsNotPlayer;

        [SerializeField] float timeIfMvpIsPlayer;

        internal void SetResults(UserResult mvp, BattleResultForClient battleResultForClient, bool mvpIsPlayer) {
            if (ShowCounter <= 0) {
                continueTimer.gameObject.SetActive(true);
                continueTimer.CurrentTime = !mvpIsPlayer ? timeIfMvpIsNotPlayer : timeIfMvpIsPlayer;
            } else {
                continueTimer.gameObject.SetActive(false);
            }

            userInfo.Set(mvp);
            mainStat.Set(mvp, battleResultForClient);
            otherStat.Set(mvp, battleResultForClient);
            tankInfo.Set(mvp);
            modulesInfo.Set(mvp.Modules);
        }

        internal void SetModuleConfig(ModuleUpgradablePowerConfigComponent moduleConfig) {
            tankInfo.SetModuleConfig(moduleConfig);
        }
    }
}