using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UniversalPriceButtonComponent : BehaviourComponent {
        [SerializeField] GameObject price;

        [SerializeField] GameObject xPrice;

        public bool PriceActivity {
            get => price.activeSelf;
            set => price.SetActive(value);
        }

        public bool XPriceActivity {
            get => xPrice.activeSelf;
            set => xPrice.SetActive(value);
        }
    }
}