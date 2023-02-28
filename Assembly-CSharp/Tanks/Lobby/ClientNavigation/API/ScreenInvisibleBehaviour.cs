using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.API {
    public class ScreenInvisibleBehaviour : ScreenBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            GetCanvasGroup(animator.gameObject).blocksRaycasts = false;
        }
    }
}