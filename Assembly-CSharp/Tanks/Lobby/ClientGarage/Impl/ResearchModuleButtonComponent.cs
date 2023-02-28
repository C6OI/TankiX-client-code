using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ResearchModuleButtonComponent : UpgradeModuleBaseButtonComponent {
        [SerializeField] protected TextMeshProUGUI cardsCountText;

        public override void Setup(int moduleLevel, int cardsCount, int maxCardCount, int price, int priceXCry, int userCryCount, int userXCryCount) {
            if (moduleLevel == -1) {
                gameObject.SetActive(true);
                Activate();
                bool flag = cardsCount >= maxCardCount;
                bool flag2 = userCryCount >= price;
                bool flag3 = flag && flag2;
                cardsCountText.text = cardsCount + "/" + maxCardCount;
                cardsCountText.color = !flag ? notEnoughTextColor : enoughTextColor;
                titleText.text = activate;
                Image image = border;
                Color color = !flag3 ? notEnoughColor : enoughColor;
                titleText.color = color;
                color = color;
                fill.color = color;
                image.color = color;
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}