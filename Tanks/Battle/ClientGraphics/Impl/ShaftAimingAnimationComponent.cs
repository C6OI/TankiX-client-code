using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class ShaftAimingAnimationComponent : MonoBehaviour, Component {
        [SerializeField] string aimingPropertyName = "isAiming";

        int aimingPropertyID;

        Animator shaftAimingAnimator;

        public void InitShaftAimingAnimation(Animator animator) {
            shaftAimingAnimator = animator;
            aimingPropertyID = Animator.StringToHash(aimingPropertyName);
        }

        public void StartAiming() => shaftAimingAnimator.SetBool(aimingPropertyID, true);

        public void StopAiming() => shaftAimingAnimator.SetBool(aimingPropertyID, false);
    }
}