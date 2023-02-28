using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class SettingsRadioButton : RadioButton {
        [SerializeField] Color defaultColor;

        [SerializeField] Color activatedColor;

        public override void Activate() {
            base.Activate();
            GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
        }

        public override void Deactivate() {
            base.Deactivate();
            GetComponentInChildren<TextMeshProUGUI>().color = defaultColor;
        }
    }
}