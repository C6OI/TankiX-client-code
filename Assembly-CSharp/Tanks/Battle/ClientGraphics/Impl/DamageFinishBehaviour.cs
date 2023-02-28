using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DamageFinishBehaviour : DamageAnimationStateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetTrigger(stopLoopID);
        }
    }
}