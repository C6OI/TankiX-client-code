using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class PlayButtonTimerComponent : MonoBehaviour {
        public delegate void TimerExpired();

        [SerializeField] TextMeshProUGUI timerTitleLabel;

        [SerializeField] TextMeshProUGUI timerTextLabel;

        [SerializeField] LocalizedField matchBeginInTitle;

        [SerializeField] LocalizedField matchBeginIn;

        [SerializeField] LocalizedField matchBeginingTitle;

        public bool isOn;

        float _ticks;

        bool matchBeginning;

        public TimerExpired onTimerExpired;

        Date startTime;

        float ticks {
            get => _ticks;
            set {
                _ticks = value;
                string text = (!matchBeginning ? matchBeginIn.Value + " " : string.Empty) + TimeToStringsConverter.SecondsToTimerFormat(value);

                if (!text.Equals(timerTextLabel.text)) {
                    timerTextLabel.text = text;
                }
            }
        }

        void Update() {
            if (!isOn || !(timerTextLabel != null)) {
                return;
            }

            ticks = startTime - Date.Now;

            if (ticks <= 0f) {
                ticks = 0f;
                isOn = false;

                if (onTimerExpired != null) {
                    onTimerExpired();
                }
            }
        }

        void OnDestroy() {
            onTimerExpired = null;
        }

        public void RunTheTimer(Date startTime, bool matchBeginnig) {
            matchBeginning = matchBeginnig;

            if (matchBeginnig) {
                timerTitleLabel.text = matchBeginingTitle.Value;
            } else {
                timerTitleLabel.text = matchBeginInTitle.Value;
            }

            this.startTime = startTime;
            ticks = startTime - Date.Now;
            isOn = true;
        }

        public void StopTheTimer() {
            isOn = false;
        }
    }
}