using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class DamageAnimationStateMachineBehaviour : StateMachineBehaviour {
        [SerializeField] string startLoopName = "startDamageLoop";

        [SerializeField] string stopLoopName = "stopDamageLoop";

        protected int startLoopID;

        protected int stopLoopID;

        void Awake() {
            startLoopID = Animator.StringToHash(startLoopName);
            stopLoopID = Animator.StringToHash(stopLoopName);
        }
    }
}