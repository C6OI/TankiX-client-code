using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DamageStartBehaviour : DamageAnimationStateMachineBehaviour {
        new void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            animator.SetTrigger(startLoopID);
    }
}