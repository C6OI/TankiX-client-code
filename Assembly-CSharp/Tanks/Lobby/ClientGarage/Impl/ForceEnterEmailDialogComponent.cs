using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ForceEnterEmailDialogComponent : ConfirmDialogComponent {
        [SerializeField] TMP_InputField inputField;

        protected override void OnEnable() {
            base.OnEnable();
            inputField.ActivateInputField();
        }
    }
}