using UnityEngine;

namespace Lobby.ClientControls.API {
    public abstract class BaseCombatLogMessageElement : MonoBehaviour {
        [SerializeField] protected RectTransform rectTransform;

        public RectTransform RectTransform => rectTransform;
    }
}