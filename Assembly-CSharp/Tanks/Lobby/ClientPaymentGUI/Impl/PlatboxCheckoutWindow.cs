using System;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientPayment.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PlatboxCheckoutWindow : MonoBehaviour {
        [SerializeField] TextMeshProUGUI transactionNumberValue;

        [SerializeField] TextMeshProUGUI priceValue;

        [SerializeField] GameObject receiptObject;

        [SerializeField] TextMeshProUGUI crystalsAmountValue;

        [SerializeField] TextMeshProUGUI specialOfferText;

        [SerializeField] TextMeshProUGUI phoneNumberValue;

        Action onForward;

        void OnDisable() {
            receiptObject.SetActive(false);
            specialOfferText.gameObject.SetActive(false);
        }

        void SetTransactionNumber(string transactionNumber) {
            transactionNumberValue.text = transactionNumber;
        }

        void SetPrice(double price, string currency) {
            priceValue.text = price.ToStringSeparatedByThousands() + " " + currency;
        }

        void SetCrystalsAmount(long amount) {
            receiptObject.SetActive(true);
            crystalsAmountValue.text = amount.ToStringSeparatedByThousands() + "<sprite=9>";
        }

        void SetSpecialOfferText(string text) {
            specialOfferText.gameObject.SetActive(true);
            specialOfferText.text = text;
        }

        void SetPhoneNumber(string phoneNumber) {
            phoneNumberValue.text = phoneNumber;
        }

        public void Show(Entity item, Entity method, string transaction, string phoneNumber, Action onForward) {
            SetPhoneNumber(phoneNumber);
            SetTransactionNumber(transaction);
            GoodsPriceComponent component = item.GetComponent<GoodsPriceComponent>();
            GoodsComponent component2 = item.GetComponent<GoodsComponent>();
            bool flag = item.HasComponent<SpecialOfferComponent>();
            string methodName = method.GetComponent<PaymentMethodComponent>().MethodName;
            double price = component.Price;
            price = !flag ? component.Round(component2.SaleState.PriceMultiplier * price) : item.GetComponent<SpecialOfferComponent>().GetSalePrice(price);

            if (item.HasComponent<XCrystalsPackComponent>()) {
                XCrystalsPackComponent component3 = item.GetComponent<XCrystalsPackComponent>();
                long num = component3.Amount;

                if (!flag) {
                    num = (long)Math.Round(component2.SaleState.AmountMultiplier * num);
                }

                SetCrystalsAmount(num + component3.Bonus);
            }

            SetPrice(price, component.Currency);
            MainScreenComponent.Instance.OverrideOnBack(Proceed);
            this.onForward = onForward;
            gameObject.SetActive(true);
        }

        public void Proceed() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            GetComponent<Animator>().SetTrigger("cancel");
            onForward();
        }
    }
}