using UnityEngine;

namespace Tanks.Lobby.ClientControls.Impl {
    public class ScreenLockShowBehaviour : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.transform.parent.SetAsLastSibling();
        }
    }
}