using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class TimerUIComponent : MonoBehaviour {
        [SerializeField] Text minutesText;

        [SerializeField] Text secondsText;

        int previousSecondsLeft;

        float secondsLeft;

        public float SecondsLeft {
            get => secondsLeft;
            set {
                secondsLeft = value;
                UpdateTextFields();
            }
        }

        public void Awake() {
            secondsLeft = 0f;
            previousSecondsLeft = -1;
        }

        bool ValidateSecondsChanging(int intSecondsLeft) {
            if (intSecondsLeft == previousSecondsLeft) {
                return false;
            }

            previousSecondsLeft = intSecondsLeft;
            return true;
        }

        void UpdateTextFields() {
            int num = (int)secondsLeft;

            if (ValidateSecondsChanging(num)) {
                if (minutesText != null) {
                    TimeSpan timeSpan = new(0, 0, 0, num);
                    secondsText.text = AddLeadingZero(timeSpan.Seconds);
                    minutesText.text = ((int)timeSpan.TotalMinutes).ToString();
                } else {
                    secondsText.text = num.ToString();
                }
            }
        }

        string AddLeadingZero(int seconds) => (seconds >= 10 ? string.Empty : "0") + seconds;
    }
}