using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoldBoxesShopTabComponent : BehaviourComponent {
        public GameObject PackPrefab;

        public Transform PackContainer;

        public TextMeshProUGUI UserBoxCount;
    }
}