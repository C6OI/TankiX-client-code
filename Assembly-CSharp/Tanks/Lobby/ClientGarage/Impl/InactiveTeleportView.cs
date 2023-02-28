using System.Text;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class InactiveTeleportView : MonoBehaviour {
        static readonly string PERCENT = "%";
        public GameObject percentText;

        public GameObject successTeleportationText;

        public TextMeshProUGUI timerText;

        public TextMeshProUGUI successTimerText;

        public LocalizedField timerTextStr;

        public Image fill;

        float durationInSec;

        Date endDate;

        StringBuilder stringBuilder;

        public void Awake() {
            stringBuilder = new StringBuilder(10);
        }

        public void Update() {
            float progress = Date.Now.GetProgress(endDate - durationInSec, durationInSec);

            if (percentText.activeSelf) {
                percentText.GetComponent<TextMeshProUGUI>().text = GetPercentText(progress);
            }

            if (fill.fillAmount > progress) {
                fill.fillAmount -= (fill.fillAmount - progress) * Time.deltaTime / 0.2f;
            } else {
                fill.fillAmount = progress;
            }

            timerText.text = GetTimerText();
            successTimerText.text = timerText.text;
        }

        public void UpdateView(Date endDate, float durationInSec, bool successTeleportation) {
            percentText.SetActive(!successTeleportation);
            successTeleportationText.SetActive(successTeleportation);
            this.endDate = endDate;
            this.durationInSec = durationInSec;
            fill.gameObject.SetActive(true);
        }

        string GetTimerText() {
            stringBuilder.Length = 0;
            stringBuilder.Append(timerTextStr);
            return TimerUtils.GetTimerText(stringBuilder, endDate - Date.Now);
        }

        string GetPercentText(float progress) {
            int num = (int)(progress * 100f);
            stringBuilder.Length = 0;
            stringBuilder.AppendFormat("{0}" + PERCENT, num);
            return stringBuilder.ToString();
        }
    }
}