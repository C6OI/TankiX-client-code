using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SpecialOfferWorthItComponent : MonoBehaviour {
        [SerializeField] TextMeshProUGUI worthItText;

        [SerializeField] LocalizedField worthItLocalizedField;

        public void SetLabel(int labelPercentage) {
            if (labelPercentage > 0) {
                gameObject.SetActive(true);
                worthItText.text = string.Format(worthItLocalizedField.Value, labelPercentage);
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}