using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class StarterPackTimerComponent : MonoBehaviour {
        public delegate void TimerExpired();

        [SerializeField] TextMeshProUGUI timerTextLabel;

        public bool isOn;

        float _ticks;

        public TimerExpired onTimerExpired;

        Date startTime;

        float ticks {
            get => _ticks;
            set {
                _ticks = value;
                string text = SecondsToHoursTimerFormat(value);

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

        public void StopTimer() {
            ticks = 0f;
            isOn = false;
        }

        public void RunTimer(float remaining) {
            startTime = Date.Now + remaining;
            ticks = startTime - Date.Now;
            isOn = true;
        }

        string SecondsToHoursTimerFormat(double seconds) {
            int num = (int)(seconds / 60.0);
            int num2 = (int)(seconds - num * 60);
            int num3 = (int)(num / 60f);
            num -= num3 * 60;
            return string.Format("{0:0}:{1:00}:{2:00}", num3, num, num2);
        }
    }
}