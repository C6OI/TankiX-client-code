using System;
using System.Collections;
using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class Receipt : LocalizedControl {
        [SerializeField] Text price;

        [SerializeField] Text total;

        [SerializeField] ReceiptItem receiptItemPrefab;

        [SerializeField] RectTransform receiptItemsContainer;

        [SerializeField] Text priceLabel;

        [SerializeField] GameObject totalObject;

        [SerializeField] Text specialOfferText;

        [SerializeField] Text totalLabel;

        long totalValue;

        public Dictionary<object, object> Lines { get; set; }

        public virtual string PriceLabel {
            set => priceLabel.text = value;
        }

        public virtual string TotalLabel {
            set => totalLabel.text = value;
        }

        void OnDisable() {
            totalValue = 0L;
            IEnumerator enumerator = receiptItemsContainer.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;

                    if (transform != specialOfferText.transform) {
                        Destroy(transform.gameObject);
                    }
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }

            totalObject.SetActive(false);
            specialOfferText.gameObject.SetActive(false);
            specialOfferText.text = string.Empty;
        }

        public void SetPrice(double price, string currency) {
            this.price.text = price.ToStringSeparatedByThousands() + " " + currency;
        }

        public void AddSpecialOfferText(string text) {
            specialOfferText.gameObject.SetActive(true);
            specialOfferText.text = text;
        }

        public void AddItem(string name, long amount) {
            totalObject.SetActive(true);
            ReceiptItem receiptItem = Instantiate(receiptItemPrefab);
            receiptItem.Init(name, amount);
            receiptItem.transform.SetParent(receiptItemsContainer, false);
            totalValue += amount;
            total.text = totalValue.ToStringSeparatedByThousands();
        }
    }
}