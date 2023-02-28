using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class TopPanelButton : MonoBehaviour {
        [SerializeField] Image filledImage;

        bool activated;

        public bool ImageFillToRight {
            set => filledImage.fillOrigin = !value ? 1 : 0;
        }

        public bool Activated {
            set {
                activated = value;
                GetComponent<Animator>().SetBool("activated", activated);
            }
        }

        void OnEnable() {
            Activated = activated;
        }

        void OnDisable() {
            Activated = false;
        }
    }
}