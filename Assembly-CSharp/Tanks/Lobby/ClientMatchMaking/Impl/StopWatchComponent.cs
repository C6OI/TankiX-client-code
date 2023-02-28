using System;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class StopWatchComponent : MonoBehaviour {
        [SerializeField] TextMeshProUGUI textLabel;

        public bool isOn;

        double startTicks;

        double ticks {
            set {
                string text = TimeToStringsConverter.SecondsToTimerFormat(value);

                if (!text.Equals(textLabel.text)) {
                    textLabel.text = text;
                }
            }
        }

        void Update() {
            if (isOn && textLabel != null) {
                ticks = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds - startTicks;
            }
        }

        public void RunTheStopwatch() {
            startTicks = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;
            isOn = true;
        }

        public void StopTheStopwatch() {
            isOn = false;
        }
    }
}