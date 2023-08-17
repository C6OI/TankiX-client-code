using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IsisAnimationWorkingStateBehaviour : StateMachineBehaviour {
        [SerializeField] string startLoopName = "startWorkingLoop";

        [SerializeField] string stopLoopName = "stopWorkingLoop";

        int startLoopID;

        int stopLoopID;

        void Awake() {
            startLoopID = Animator.StringToHash(startLoopName);
            stopLoopID = Animator.StringToHash(stopLoopName);
        }

        new void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) =>
            animator.SetTrigger(startLoopID);

        new void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) =>
            animator.SetTrigger(stopLoopID);
    }
}