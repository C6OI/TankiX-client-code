using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class EnterConfirmationCodeScreenComponent : LocalizedScreenComponent, NoScaleScreen {
        [SerializeField] TextMeshProUGUI confirmationHintWithUserEmail;

        [SerializeField] TextMeshProUGUI confirmationCodeText;

        [SerializeField] ConfirmationCodeSendAgainComponent confirmationCodeSendAgainComponent;

        [SerializeField] Color emailColor = Color.green;

        public string ConfirmationHintWithUserEmail {
            set => confirmationHintWithUserEmail.text = value;
        }

        public string ConfirmationHint { get; set; }

        public string ConfirmationCodeText {
            get => confirmationCodeText.text;
            set => confirmationCodeText.text = value;
        }

        public string InvalidCodeMessage { get; set; }

        public Color EmailColor => emailColor;

        public void ResetSendAgainTimer(long emailSendThresholdInSeconds) {
            if (confirmationCodeSendAgainComponent != null) {
                confirmationCodeSendAgainComponent.ShowTimer(emailSendThresholdInSeconds);
            }
        }
    }
}