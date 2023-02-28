using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapAnimatorTriggerComponent : MonoBehaviour {
        public Transform animator;

        public string triggerEnable;

        public string triggerDisable;

        Animator animatorController;

        int count;

        void Start() {
            animatorController = animator.GetComponent<Animator>();
        }

        void OnTriggerEnter() {
            if (count == 0) {
                animatorController.SetBool(triggerEnable, true);
            }

            count++;
        }

        void OnTriggerExit() {
            if (count == 1) {
                animatorController.SetBool(triggerEnable, false);
            }

            count--;
        }
    }
}