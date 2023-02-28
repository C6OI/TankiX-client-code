using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class RewardContainerHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        public Animator animator;

        public void OnPointerEnter(PointerEventData eventData) {
            animator.SetTrigger("Enter");
        }

        public void OnPointerExit(PointerEventData eventData) {
            animator.SetTrigger("Exit");
        }
    }
}