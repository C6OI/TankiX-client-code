using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class GameModeSelectScreenComponent : BehaviourComponent {
        [SerializeField] GameObject gameModeItemPrefab;

        [SerializeField] GameObject gameModesContainer;

        [SerializeField] GameObject mainGameModeContainer;

        [SerializeField] TextMeshProUGUI mmLevel;

        public GameObject GameModeItemPrefab => gameModeItemPrefab;

        public GameObject GameModesContainer => gameModesContainer;

        public GameObject MainGameModeContainer => mainGameModeContainer;

        public int MMLevel {
            set => mmLevel.text = value.ToString();
        }
    }
}