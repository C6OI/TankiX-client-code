using UnityEngine;

namespace Lobby.ClientNavigation.API {
    public class ScreenInactiveBehaviour : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            animator.gameObject.SetActive(false);
    }
}