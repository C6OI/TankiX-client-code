using Lobby.ClientPayment.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class BankCardPaymentScreenComponent : BasePaymentScreenComponent {
        [SerializeField] Text cardRequisitesLabel;

        [SerializeField] Text cardNumberLabel;

        [SerializeField] Text cardExpirationDateLabel;

        [SerializeField] Text cardHolderLabel;

        [SerializeField] Text cardCVVLabel;

        [SerializeField] Text cardCVVHint;

        [SerializeField] InputField number;

        [SerializeField] InputField mm;

        [SerializeField] InputField yy;

        [SerializeField] InputField cardHolder;

        [SerializeField] InputField cvc;

        public virtual string CardRequisitesLabel {
            set => cardRequisitesLabel.text = value;
        }

        public virtual string CardNumberLabel {
            set => cardNumberLabel.text = value;
        }

        public virtual string CardExpirationDateLabel {
            set => cardExpirationDateLabel.text = value;
        }

        public virtual string CardHolderLabel {
            set => cardHolderLabel.text = value;
        }

        public virtual string CardCVVLabel {
            set => cardCVVLabel.text = value;
        }

        public virtual string CardCVVHint {
            set => cardCVVHint.text = value;
        }

        public string Number => number.text;

        public string MM => mm.text;

        public string YY => yy.text;

        public string CardHolder => cardHolder.text;

        public string CVC => cvc.text;

        protected override void Awake() {
            base.Awake();
            cvc.onValueChanged.AddListener(ValidateInput);
            cardHolder.onValueChanged.AddListener(ValidateInput);
            number.onValueChanged.AddListener(ValidateInput);
            mm.onValueChanged.AddListener(ValidateInput);
            yy.onValueChanged.AddListener(ValidateInput);
        }

        void OnEnable() {
            cvc.text = string.Empty;
            cardHolder.text = string.Empty;
            number.text = string.Empty;
            mm.text = string.Empty;
            yy.text = string.Empty;
            ValidateInput(string.Empty);
        }

        void ValidateInput(string input = "") {
            bool flag = cvc.text.Length == cvc.characterLimit &&
                        BankCardUtils.IsBankCard(number.text) &&
                        yy.text.Length == yy.characterLimit &&
                        !string.IsNullOrEmpty(cardHolder.text);

            if (flag) {
                int num = int.Parse(mm.text);
                flag = flag && num >= 1 && num <= 12;
            }

            continueButton.interactable = flag;
        }
    }
}