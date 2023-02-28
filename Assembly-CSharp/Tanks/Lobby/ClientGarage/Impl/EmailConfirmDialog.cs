using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class EmailConfirmDialog : ConfirmDialogComponent {
        [SerializeField] TextMeshProUGUI confirmationHintLabel;

        [SerializeField] LocalizedField confirmationHint;

        [SerializeField] PaletteColorField emailColor;

        public void ShowEmailConfirm(string email) {
            confirmationHintLabel.text = confirmationHint.Value.Replace("%EMAIL%", "<color=#" + emailColor.Color.ToHexString() + ">" + email + "</color>");
        }
    }
}