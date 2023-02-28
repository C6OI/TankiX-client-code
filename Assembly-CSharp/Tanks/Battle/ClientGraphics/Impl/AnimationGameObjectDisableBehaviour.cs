using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AnimationGameObjectDisableBehaviour : MonoBehaviour {
        public new GameObject gameObject;

        public void DisableGameObject() {
            gameObject.SetActive(false);
        }
    }
}