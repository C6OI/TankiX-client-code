using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SpeedStateBehaviour : StateMachineBehaviour {
        [SerializeField] string startSpeedLoopName = "startSpeedLoop";

        [SerializeField] string stopSpeedLoopName = "stopSpeedLoop";

        int startSpeedLoopID;

        int stopSpeedLoopID;

        void Awake() {
            startSpeedLoopID = Animator.StringToHash(startSpeedLoopName);
            stopSpeedLoopID = Animator.StringToHash(stopSpeedLoopName);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetTrigger(startSpeedLoopID);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetTrigger(stopSpeedLoopID);
        }
    }
}