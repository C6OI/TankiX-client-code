using Lobby.ClientNavigation.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1447755772977L)]
    public class ItemPropertiesScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text rewardsText;

        public GameObject UpgradeButton;

        public ItemAttributesComponent itemAttribute;

        public string RewardsText {
            set => rewardsText.text = value;
        }
    }
}