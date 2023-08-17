using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientUserProfile.Impl {
    public class ConfirmUserEmailScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text confirmationHintWithUserEmail;

        [SerializeField] Text sendNewsText;

        [SerializeField] Text confirmText;

        [SerializeField] Text sendAgainText;

        [SerializeField] Text rightPanelHint;

        [SerializeField] Text confirmationCodeText;

        [SerializeField] Color emailColor = Color.green;

        [SerializeField] GameObject cancelButton;

        [SerializeField] GameObject changeEmailButton;

        public string ConfirmationHintWithUserEmail {
            set => confirmationHintWithUserEmail.text = value;
        }

        public string ConfirmationHint { get; set; }

        public string SendNewsText {
            set => sendNewsText.text = value;
        }

        public string ConfirmText {
            set => confirmText.text = value.ToUpper();
        }

        public string SendAgainText {
            set => sendAgainText.text = value.ToUpper();
        }

        public string RightPanelHint {
            set => rightPanelHint.text = value;
        }

        public string ConfirmationCodeText {
            set => confirmationCodeText.text = value;
        }

        public string InvalidCodeMessage { get; set; }

        public Color EmailColor => emailColor;

        void OnEnable() {
            cancelButton.SetActive(false);
            changeEmailButton.SetActive(true);
            rightPanelHint.gameObject.SetActive(true);
        }

        public void ActivateCancel() {
            cancelButton.SetActive(true);
            changeEmailButton.SetActive(false);
            rightPanelHint.gameObject.SetActive(false);
        }
    }
}