using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CantUseAllEnergyBonusDialog : ConfirmWindowComponent {
        [SerializeField] TextMeshProUGUI question;

        [SerializeField] LocalizedField questionText;

        public void SetEnergyCount(long energy) {
            question.text = string.Format(questionText.Value, energy);
        }
    }
}