using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class QiwiWalletScreenComponent : LocalizedScreenComponent, PaymentScreen {
        [SerializeField] InputField account;

        [SerializeField] Receipt receipt;

        [SerializeField] Text accountText;

        [SerializeField] Text continueButton;

        [SerializeField] Button button;

        [SerializeField] QiwiAccountFormatterComponent formatter;

        public string Account => "+" + account.text.Replace(" ", string.Empty);

        public string ErrorText { get; set; }

        public Receipt Receipt => receipt;

        public string AccountText {
            set => accountText.text = value;
        }

        public string ContinueButton {
            set => continueButton.text = value;
        }

        protected override void Awake() {
            base.Awake();
            account.onValueChanged.AddListener(Check);
            Check(account.text);
        }

        void Check(string text) {
            button.interactable = formatter.IsValidPhoneNumber;
        }

        public void DisableContinueButton() {
            button.interactable = false;
        }
    }
}