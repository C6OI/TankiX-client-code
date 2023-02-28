using System;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Animator))]
    public class PlayButtonComponent : EventMappingComponent {
        public GameObject cancelSearchButton;

        public GameObject goToLobbyButton;

        public GameObject exitLobbyButton;

        public MatchSearchingGUIComponent matchSearchingGui;

        [SerializeField] LocalizedField playersInLobby;

        [SerializeField] TextMeshProUGUI gameModeTitleLabel;

        [SerializeField] TextMeshProUGUI playersInLobbyLabel;

        Animator animator;

        string lastAnimatorTrigger;

        public Animator Animator => animator ?? (animator = GetComponent<Animator>());

        public bool SearchingDefaultGameMode { get; set; }

        void OnEnable() {
            if (!string.IsNullOrEmpty(lastAnimatorTrigger)) {
                SetAnimatorTrigger(lastAnimatorTrigger);
            }
        }

        public void SetAnimatorTrigger(string trigger) {
            lastAnimatorTrigger = trigger;

            if (Animator.isActiveAndEnabled) {
                AnimatorControllerParameter[] parameters = Animator.parameters;

                foreach (AnimatorControllerParameter animatorControllerParameter in parameters) {
                    Animator.SetBool(animatorControllerParameter.name, false);
                }

                Animator.SetBool(lastAnimatorTrigger, true);
            }
        }

        protected override void Subscribe() {
            PlayButtonTimerComponent component = GetComponent<PlayButtonTimerComponent>();
            component.onTimerExpired = (PlayButtonTimerComponent.TimerExpired)Delegate.Combine(component.onTimerExpired, new PlayButtonTimerComponent.TimerExpired(OnTimerExpired));
        }

        public void InitializeMatchSearchingWaitTime(bool newbieMode) {
            matchSearchingGui.SetWaitingTime(newbieMode);
        }

        public void RunTheStopwatch() {
            StopTheTimer();
            GetComponent<StopWatchComponent>().RunTheStopwatch();
        }

        public void StopTheStopwatch() {
            GetComponent<StopWatchComponent>().StopTheStopwatch();
        }

        public void RunTheTimer(Date startTime, bool matchBeginning = false) {
            StopTheStopwatch();
            GetComponent<PlayButtonTimerComponent>().RunTheTimer(startTime, matchBeginning);
        }

        public void StopTheTimer() {
            GetComponent<PlayButtonTimerComponent>().StopTheTimer();
        }

        public void ShowCancelButton(bool show) {
            cancelSearchButton.SetActive(show);
        }

        public void ShowGoToLobbyButton(bool show) {
            goToLobbyButton.SetActive(show);
        }

        public void ShowExitLobbyButton(bool show) {
            exitLobbyButton.SetActive(show);
        }

        void OnTimerExpired() {
            SendEvent<PlayButtonTimerExpiredEvent>();
        }

        public void SetCustomModeTitle(string modeName, int currentPlayersCount, int maxPlayersCount) {
            gameModeTitleLabel.text = modeName;
            playersInLobbyLabel.text = playersInLobby.Value + "\n" + currentPlayersCount + "/" + maxPlayersCount;
        }
    }
}