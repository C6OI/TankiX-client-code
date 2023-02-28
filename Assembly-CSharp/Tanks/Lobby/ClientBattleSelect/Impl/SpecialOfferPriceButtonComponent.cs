using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SpecialOfferPriceButtonComponent : BehaviourComponent {
        [SerializeField] string priceRegularFormatting;

        [SerializeField] string priceDiscountedFormatting;

        [SerializeField] TextMeshProUGUI priceText;

        public void SetPrice(int price, string currency) {
            SetPrice(price, 0, currency);
        }

        public void SetPrice(double price, int discount, string currency) {
            if (discount != 0) {
                priceText.text = string.Format(priceDiscountedFormatting, discount, price, currency);
            } else {
                priceText.text = string.Format(priceRegularFormatting, price, currency);
            }
        }
    }
}