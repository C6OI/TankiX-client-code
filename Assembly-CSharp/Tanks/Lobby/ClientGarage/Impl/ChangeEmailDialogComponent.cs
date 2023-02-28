using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ChangeEmailDialogComponent : ConfirmDialogComponent {
        [SerializeField] GameObject input;

        [SerializeField] GameObject confirm;

        [SerializeField] Button button;

        [SerializeField] TextMeshProUGUI hintLabel;

        [SerializeField] TextMeshProUGUI confirmationHintLabel;

        [SerializeField] LocalizedField hint;

        [SerializeField] LocalizedField confirmationHint;

        [SerializeField] PaletteColorField emailColor;

        [SerializeField] TMP_InputField inputField;

        protected override void OnEnable() {
            base.OnEnable();
            ShowInput();
            button.interactable = false;
        }

        void ShowInput() {
            confirm.SetActive(false);
            input.SetActive(true);
            inputField.ActivateInputField();
        }

        public void ShowEmailConfirm(string email) {
            confirmationHintLabel.text = confirmationHint.Value.Replace("%EMAIL%", "<color=#" + emailColor.Color.ToHexString() + ">" + email + "</color>");
            input.SetActive(false);
            confirm.SetActive(true);
        }

        public void SetActiveHint(bool value) {
            hintLabel.text = !value ? string.Empty : hint.Value;
            hintLabel.rectTransform.sizeDelta = new Vector2(hintLabel.rectTransform.sizeDelta.x, !value ? 30 : 80);
        }
    }
}