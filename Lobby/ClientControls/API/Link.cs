using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public class Link : MonoBehaviour {
        [SerializeField] Animator animator;

        [SerializeField] Button button;

        public void Awake() => button.onClick.AddListener(OnClick);

        public void OnClick() {
            button.onClick.RemoveListener(OnClick);
            animator.SetBool("Activated", true);
        }
    }
}