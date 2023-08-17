using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientUserProfile.Impl {
    public class EnterConfirmationCodeScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text continueButton;

        [SerializeField] Text confirmationHintWithUserEmail;

        [SerializeField] Text sendAgainText;

        [SerializeField] Text confirmationCodeText;

        [SerializeField] Color emailColor = Color.green;

        public virtual string ContinueButton {
            set => continueButton.text = value.ToUpper();
        }

        public string ConfirmationHintWithUserEmail {
            set => confirmationHintWithUserEmail.text = value;
        }

        public string ConfirmationHint { get; set; }

        public string SendAgainText {
            set => sendAgainText.text = value.ToUpper();
        }

        public string ConfirmationCodeText {
            get => confirmationCodeText.text;
            set => confirmationCodeText.text = value;
        }

        public string InvalidCodeMessage { get; set; }

        public Color EmailColor => emailColor;
    }
}