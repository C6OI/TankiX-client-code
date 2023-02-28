using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestsScreenComponent : BehaviourComponent {
        [SerializeField] GameObject questPrefab;

        [SerializeField] GameObject questCellPrefab;

        [SerializeField] GameObject questsContainer;

        public GameObject QuestPrefab => questPrefab;

        public GameObject QuestCellPrefab => questCellPrefab;

        public GameObject QuestsContainer => questsContainer;
    }
}