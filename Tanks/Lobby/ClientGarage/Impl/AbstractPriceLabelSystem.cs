using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AbstractPriceLabelSystem : ECSSystem {
        const long ROUNDED_AFTER_NUMBER = 9999999L;

        protected void UpdatePriceForUser(long priceValue, AbstractPriceLabelComponent priceLabel, long userMoney) {
            Color color = userMoney >= priceValue ? priceLabel.DefualtColor : priceLabel.shortageColor;
            UpdatePrice(priceValue, priceLabel, color);
        }

        protected void UpdatePrice(long priceValue, AbstractPriceLabelComponent priceLabel, Color color) {
            priceLabel.Text.color = color;
            string text = priceValue.ToString();

            if (priceValue > 9999999) {
                text = RoundPrice(priceValue);
            }

            priceLabel.Text.text = text;
            priceLabel.Price = priceValue;
        }

        string RoundPrice(long price) {
            int num = 0;
            string text = string.Empty;

            while (price >= 1000) {
                price /= 1000;
                num++;
                text += "K";
            }

            return price + text;
        }
    }
}