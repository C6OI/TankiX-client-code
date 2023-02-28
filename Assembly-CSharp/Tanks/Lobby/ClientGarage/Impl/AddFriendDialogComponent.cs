using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AddFriendDialogComponent : ConfirmDialogComponent {
        [SerializeField] TMP_InputField inputField;

        protected override void OnEnable() {
            base.OnEnable();
            ShowInput();
        }

        void ShowInput() {
            inputField.ActivateInputField();
        }
    }
}