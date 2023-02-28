using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(Animator))]
    public class CombatEventLogMessage : BaseCombatLogMessageElement {
        [SerializeField] float messageTimeout;

        [SerializeField] LayoutElement layoutElement;

        [SerializeField] Animator animator;

        [SerializeField] protected RectTransform placeholder;

        bool deleteRequested;

        protected RectTransform rightElement;

        public LayoutElement LayoutElement => layoutElement;

        void SendScroll() {
            SendMessageUpwards("OnScrollLog", layoutElement.preferredHeight);
        }

        void Delete() {
            SendMessageUpwards("OnDeleteMessage", this);
        }

        public void RequestDelete() {
            if (!deleteRequested && (bool)animator) {
                deleteRequested = true;
                animator.SetTrigger("Hide");
            }
        }

        public void ShowMessage() {
            animator.SetTrigger("Show");
        }

        public virtual void Attach(RectTransform child, bool toRight) {
            child.SetParent(placeholder, false);

            if (toRight) {
                if (rightElement != null) {
                    rightElement.SetParent(child, false);
                    LayoutElement layoutElement = rightElement.GetComponent<LayoutElement>() ?? rightElement.gameObject.AddComponent<LayoutElement>();
                    layoutElement.ignoreLayout = true;
                }

                rightElement = child;
            }
        }
    }
}