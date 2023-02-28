using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MobilePaymentCheckoutScreenComponent : LocalizedScreenComponent, PaymentScreen {
        [SerializeField] Text paymentMethodLabel;

        [SerializeField] Text paymentMethodValue;

        [SerializeField] Text successLabel;

        [SerializeField] Text transactionNumberLabel;

        [SerializeField] Text transactionNumberValue;

        [SerializeField] Text priceLabel;

        [SerializeField] Text priceValue;

        [SerializeField] Text crystalsAmountLabel;

        [SerializeField] GameObject receiptObject;

        [SerializeField] Text crystalsAmountValue;

        [SerializeField] Text specialOfferText;

        [SerializeField] Text phoneNumberLabel;

        [SerializeField] Text phoneNumberValue;

        [SerializeField] Text aboutLabel;

        [SerializeField] Text rightPanelHint;

        public virtual string PaymentMethodLabel {
            set => paymentMethodLabel.text = value;
        }

        public virtual string PaymentMethodValue {
            set => paymentMethodValue.text = value;
        }

        public virtual string SuccessLabel {
            set => successLabel.text = value;
        }

        public virtual string TransactionNumberLabel {
            set => transactionNumberLabel.text = value;
        }

        public virtual string PriceLabel {
            set => priceLabel.text = value;
        }

        public virtual string CrystalsAmountLabel {
            set => crystalsAmountLabel.text = value;
        }

        public virtual string PhoneNumberLabel {
            set => phoneNumberLabel.text = value;
        }

        public virtual string AboutLabel {
            set => aboutLabel.text = value;
        }

        public virtual string RightPanelHint {
            set => rightPanelHint.text = value;
        }

        void OnDisable() {
            receiptObject.SetActive(false);
            specialOfferText.gameObject.SetActive(false);
        }

        public void SetTransactionNumber(string transactionNumber) {
            transactionNumberValue.text = transactionNumber;
        }

        public void SetPrice(double price, string currency) {
            priceValue.text = price.ToStringSeparatedByThousands() + " " + currency;
        }

        public void SetCrystalsAmount(long amount) {
            receiptObject.SetActive(true);
            crystalsAmountValue.text = amount.ToStringSeparatedByThousands();
        }

        public void SetSpecialOfferText(string text) {
            specialOfferText.gameObject.SetActive(true);
            specialOfferText.text = text;
        }

        public void SetPhoneNumber(string phoneNumber) {
            phoneNumberValue.text = phoneNumber;
        }
    }
}