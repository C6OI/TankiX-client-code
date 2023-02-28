using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class QuestsResultUI : MonoBehaviour {
        [SerializeField] GameObject resultsContainer;

        [SerializeField] GameObject questResultPrefab;

        public void AddQuest(Entity quest) {
            GameObject gameObject = Instantiate(questResultPrefab);
            gameObject.transform.SetParent(resultsContainer.transform, false);
        }
    }
}