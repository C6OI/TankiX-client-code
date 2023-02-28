using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DamageStartBehaviour : DamageAnimationStateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetTrigger(startLoopID);
        }
    }
}