using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    public class ScreenLockComponent : MonoBehaviour, Component {
        [SerializeField] Animator animator;

        public void Lock() {
            animator.gameObject.SetActive(false);
            animator.gameObject.SetActive(true);
        }

        public void Unlock() {
            if (animator.gameObject.activeSelf) {
                animator.SetTrigger("Unlock");
            }
        }
    }
}