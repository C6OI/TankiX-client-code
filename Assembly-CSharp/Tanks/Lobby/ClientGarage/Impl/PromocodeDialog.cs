using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PromocodeDialog : ConfirmDialogComponent {
        [SerializeField] Button button;

        [SerializeField] TMP_InputField inputField;

        protected override void OnEnable() {
            base.OnEnable();
            button.interactable = false;
            inputField.ActivateInputField();
        }
    }
}