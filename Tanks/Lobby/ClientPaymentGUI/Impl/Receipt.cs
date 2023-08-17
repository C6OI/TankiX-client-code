using System.Collections.Generic;
using Lobby.ClientControls.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class Receipt : LocalizedControl {
        [SerializeField] Text price;

        [SerializeField] Text total;

        [SerializeField] ReceiptItem receiptItemPrefab;

        [SerializeField] RectTransform receiptItemsContainer;

        [SerializeField] Text priceLabel;

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
            receiptItemsContainer.DestroyChildren();
        }

        public void SetPrice(double price, string currency) =>
            this.price.text = price.ToStringSeparatedByThousands() + " " + currency;

        public void AddItem(string name, long amount) {
            ReceiptItem receiptItem = Instantiate(receiptItemPrefab);
            receiptItem.Init(name, amount);
            receiptItem.transform.SetParent(receiptItemsContainer, false);
            totalValue += amount;
            total.text = totalValue.ToStringSeparatedByThousands();
        }
    }
}