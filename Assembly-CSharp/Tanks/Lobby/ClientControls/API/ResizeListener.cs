using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public abstract class ResizeListener : MonoBehaviour {
        public abstract void OnResize(RectTransform source);
    }
}