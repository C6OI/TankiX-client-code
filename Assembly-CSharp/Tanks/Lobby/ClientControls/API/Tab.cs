using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class Tab : MonoBehaviour {
        [SerializeField] protected RadioButton button;

        public bool show { get; set; }

        protected virtual void OnEnable() {
            GetComponent<CanvasGroup>().alpha = 0f;
            GetComponent<Animator>().SetBool("show", true);
        }

        public virtual void Show() {
            show = true;
            button.Activate();

            if (gameObject.activeInHierarchy) {
                OnEnable();
            } else {
                gameObject.SetActive(true);
            }
        }

        public virtual void Hide() {
            show = false;

            if (gameObject.activeInHierarchy) {
                GetComponent<Animator>().SetBool("show", false);
            } else {
                gameObject.SetActive(false);
            }

            SendMessage("OnHide", SendMessageOptions.DontRequireReceiver);
        }

        public virtual void OnHid() {
            if (show) {
                OnEnable();
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}