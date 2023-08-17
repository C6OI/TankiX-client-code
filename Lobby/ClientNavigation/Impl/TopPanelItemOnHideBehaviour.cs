using UnityEngine;

namespace Lobby.ClientNavigation.Impl {
    public class TopPanelItemOnHideBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            animator.gameObject.SetActive(false);
    }
}