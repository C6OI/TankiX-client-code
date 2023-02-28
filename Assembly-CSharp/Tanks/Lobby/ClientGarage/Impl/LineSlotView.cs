using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LineSlotView : MonoBehaviour {
        [SerializeField] Image longLine;

        [SerializeField] Image shortLine;

        void OnDisable() {
            longLine.fillAmount = 0f;
            shortLine.fillAmount = 0f;
        }
    }
}