using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class DroneAnimatorComponent : MonoBehaviour, Component {
        [SerializeField] string idleStateName = "idle";

        [SerializeField] string shootStateName = "shot";

        int idleStateIndex;

        int shootStateIndex;

        Animator vulcanAnimator;

        void Awake() {
            vulcanAnimator = GetComponent<Animator>();
            idleStateIndex = Animator.StringToHash(idleStateName);
            shootStateIndex = Animator.StringToHash(shootStateName);
        }

        public void StartIdle() {
            vulcanAnimator.SetTrigger(idleStateIndex);
        }

        public void StartShoot() {
            vulcanAnimator.SetTrigger(shootStateIndex);
        }
    }
}