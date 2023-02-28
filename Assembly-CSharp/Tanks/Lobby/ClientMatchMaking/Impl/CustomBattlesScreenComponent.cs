using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class CustomBattlesScreenComponent : BehaviourComponent {
        public GameObject GameModeItemPrefab;

        public GameObject GameModesContainer;

        public void ScrollToTheLeft() {
            GetComponentInChildren<ScrollRect>().horizontalNormalizedPosition = 0f;
        }
    }
}