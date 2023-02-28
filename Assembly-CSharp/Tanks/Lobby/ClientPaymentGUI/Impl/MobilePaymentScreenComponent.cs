using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MobilePaymentScreenComponent : BasePaymentScreenComponent, PaymentScreen {
        [SerializeField] Text mobilePhoneLabel;

        [SerializeField] Text phoneCountryCode;

        [SerializeField] InputField phoneNumber;

        public virtual string MobilePhoneLabel {
            set => mobilePhoneLabel.text = value;
        }

        public virtual string PhoneCountryCode {
            get => phoneCountryCode.text;
            set => phoneCountryCode.text = value;
        }

        public virtual string PhoneNumber {
            get => phoneNumber.text;
            set => phoneNumber.text = value;
        }

        protected override void Awake() {
            base.Awake();
            phoneNumber.onValueChanged.AddListener(ValidateInput);
        }

        void OnEnable() {
            phoneNumber.text = string.Empty;
            ValidateInput(string.Empty);
        }

        void ValidateInput(string input = "") {
            continueButton.interactable = phoneNumber.text.Length == phoneNumber.characterLimit;
        }
    }
}