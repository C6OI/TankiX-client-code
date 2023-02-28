using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class VisualUIListSwitch : MonoBehaviour {
        public void OnEnable() {
            GetComponent<Animator>().SetTrigger("switch");
        }

        public void Switch() {
            GetComponentInParent<VisualUI>().Switch();
        }

        public void Animate() {
            if (gameObject.activeInHierarchy) {
                GetComponent<Animator>().SetTrigger("switch");
            }
        }
    }
}