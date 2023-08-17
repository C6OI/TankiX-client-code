using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(Animator))]
    public class CombatEventLogMessage : BaseCombatLogMessageElement {
        [SerializeField] float messageTimeout;

        [SerializeField] LayoutElement layoutElement;

        [SerializeField] Animator animator;

        [SerializeField] RectTransform placeholder;

        bool deleteRequested;

        RectTransform rightElement;

        public LayoutElement LayoutElement => layoutElement;

        void SendScroll() => SendMessageUpwards("OnScrollLog", layoutElement.preferredHeight);

        void Delete() => SendMessageUpwards("OnDeleteMessage", this);

        public void RequestDelete() {
            if (!deleteRequested) {
                deleteRequested = true;
                animator.SetTrigger("Hide");
            }
        }

        public void ShowMessage() => animator.SetTrigger("Show");

        public void AttachToRight(RectTransform child) {
            child.SetParent(placeholder, false);

            if (rightElement != null) {
                rightElement.SetParent(child, false);

                LayoutElement layoutElement = rightElement.GetComponent<LayoutElement>() ??
                                              rightElement.gameObject.AddComponent<LayoutElement>();

                layoutElement.ignoreLayout = true;
            }

            rightElement = child;
        }
    }
}