using System.Collections;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.Impl {
    public class AnimationTriggerDelayBehaviour : MonoBehaviour {
        public float dealy;

        public Animator animator;

        public string trigger;

        void Start() {
            StartCoroutine(ExecuteAfterTime(dealy));
        }

        IEnumerator ExecuteAfterTime(float time) {
            yield return new WaitForSeconds(time);

            animator.SetTrigger(trigger);
        }
    }
}