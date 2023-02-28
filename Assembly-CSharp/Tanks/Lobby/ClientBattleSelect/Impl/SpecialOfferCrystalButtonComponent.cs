using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SpecialOfferCrystalButtonComponent : BehaviourComponent {
        [SerializeField] string priceRegularFormatting;

        [SerializeField] string priceDiscountedFormatting;

        [SerializeField] TextMeshProUGUI priceText;

        [SerializeField] string blueCrystalIconString;

        [SerializeField] string xCrystalIconString;

        public void SetPrice(int price, bool xCry) {
            SetPrice(price, 0, xCry);
        }

        public void SetPrice(int price, int discount, bool xCry) {
            if (discount != 0) {
                priceText.text = string.Format(priceDiscountedFormatting, discount, price);
            } else {
                priceText.text = string.Format(priceRegularFormatting, price);
            }

            AddCrystals(xCry);
        }

        void AddCrystals(bool xCry) {
            if (xCry) {
                priceText.text += xCrystalIconString;
            } else {
                priceText.text += blueCrystalIconString;
            }
        }
    }
}