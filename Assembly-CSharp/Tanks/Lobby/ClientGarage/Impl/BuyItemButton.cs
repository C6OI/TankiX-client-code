using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BuyItemButton : MonoBehaviour {
        [SerializeField] GaragePrice enabledPrice;

        [SerializeField] GaragePrice disabledPrice;

        [SerializeField] Button button;

        public Button Button => button;

        public void SetPrice(int oldPrice, int price) {
            enabledPrice.NeedUpdateColor = true;
            disabledPrice.NeedUpdateColor = false;
            enabledPrice.SetPrice(oldPrice, price);
            disabledPrice.SetPrice(oldPrice, price);
        }
    }
}