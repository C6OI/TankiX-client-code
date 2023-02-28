using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AbstractPriceLabelSystem : ECSSystem {
        protected void UpdatePriceForUser(long priceValue, long oldPriceValue, AbstractPriceLabelComponent priceLabel, long userMoney) {
            priceLabel.Text.color = userMoney >= priceValue ? priceLabel.DefaultColor : priceLabel.shortageColor;
            priceLabel.Text.text = GetPriceString(priceValue);
            priceLabel.Price = priceValue;
            priceLabel.OldPriceVisible = oldPriceValue > 0 && oldPriceValue != priceValue;
            priceLabel.OldPrice = oldPriceValue;
            priceLabel.OldPriceText = GetPriceString(oldPriceValue);
        }

        string GetPriceString(long priceValue) {
            string result = priceValue.ToString();

            if (priceValue > 9999999) {
                result = RoundPrice(priceValue);
            }

            return result;
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