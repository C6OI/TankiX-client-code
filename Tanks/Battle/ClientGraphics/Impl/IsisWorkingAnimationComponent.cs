using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class IsisWorkingAnimationComponent : MonoBehaviour, Component {
        [SerializeField] string workingName = "isWorking";

        Animator isisAnimator;

        int workingID;

        public void InitIsisWorkingAnimation(Animator animator) {
            isisAnimator = animator;
            workingID = Animator.StringToHash(workingName);
        }

        public void StartWorkingAnimation() => isisAnimator.SetBool(workingID, true);

        public void StopWorkingAnimation() => isisAnimator.SetBool(workingID, false);
    }
}