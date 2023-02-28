using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class GameObjectKeyCodeEnabler : MonoBehaviour {
        public KeyCode keyCode;

        public new GameObject gameObject;

        void Update() {
            if (Input.GetKeyDown(keyCode)) {
                gameObject.SetActive(!gameObject.activeInHierarchy);
            }
        }
    }
}