using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class ObjectDeactivator : MonoBehaviour {
        public Quality.QualityLevel maxQualityForDeactivating;

        void Awake() {
            int qualityLevel = QualitySettings.GetQualityLevel();

            if (qualityLevel <= (int)maxQualityForDeactivating) {
                gameObject.SetActive(false);
            }
        }
    }
}