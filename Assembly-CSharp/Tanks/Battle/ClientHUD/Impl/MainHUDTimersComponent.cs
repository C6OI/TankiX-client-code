using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class MainHUDTimersComponent : BehaviourComponent {
        [SerializeField] Timer timer;

        [SerializeField] Timer warmingUpTimer;

        [SerializeField] Animator hudAnimator;

        [SerializeField] GameObject warmingUpTimerContainer;

        [SerializeField] GameObject mainTimerContainer;

        public Timer Timer => timer;

        public Timer WarmingUpTimer => warmingUpTimer;

        void OnDisable() {
            HideWarmingUpTimer();
        }

        public void ShowWarmingUpTimer() {
            warmingUpTimerContainer.SetActive(true);
            mainTimerContainer.SetActive(false);
        }

        public void HideWarmingUpTimer() {
            warmingUpTimerContainer.SetActive(false);
            mainTimerContainer.SetActive(true);
        }
    }
}