using Lobby.ClientControls.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.API {
    public class TimeProgressBar : MonoBehaviour {
        public Text text;

        public Image icon;

        bool active;

        Blinker blinker;

        int blinkingInterval;

        ProgressBar progressBar;

        float roundStartTime;

        float totalRoundTime;

        public void Awake() {
            blinker = gameObject.GetComponent<Blinker>();
            progressBar = gameObject.GetComponent<ProgressBar>();

            blinker.onBlink.AddListener(delegate(float value) {
                Color color = text.color;
                color.a = value;
                text.color = color;
                color = icon.color;
                color.a = value;
                icon.color = color;
            });
        }

        public void Update() {
            if (active) {
                float num = totalRoundTime - (Date.Now.UnityTime - roundStartTime);
                progressBar.ProgressValue = Mathf.Clamp01(num / totalRoundTime);
                text.text = TimerUtils.GetTimerText(num);

                if (num <= blinkingInterval && !blinker.enabled) {
                    blinker.StartBlink();
                }
            }
        }

        public void SetTotalRoundTime(float totalRoundTime) => text.text = TimerUtils.GetTimerText(totalRoundTime);

        public void Activate(float roundStartTime, float totalRoundTime, int blinkingInterval) {
            this.roundStartTime = roundStartTime;
            this.totalRoundTime = totalRoundTime;
            active = true;
            this.blinkingInterval = blinkingInterval;
        }

        public void Deactivate() {
            text.text = TimerUtils.GetTimerText(totalRoundTime);
            active = false;
            Color color = text.color;
            color.a = blinker.maxValue;
            text.color = color;
            color = icon.color;
            color.a = blinker.maxValue;
            icon.color = color;
            blinker.StopBlink();
            progressBar.ProgressValue = 0f;
        }
    }
}