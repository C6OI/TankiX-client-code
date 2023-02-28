using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class RailgunAnimationComponent : MonoBehaviour, Component {
        [SerializeField] string railgunChargingTriggerName = "railgunCharging";

        [SerializeField] string railgunStartReloadingName = "startReloading";

        [SerializeField] string railgunStopReloadingName = "stopReloading";

        [SerializeField] string railgunStopAnimationName = "stopAnyActions";

        [SerializeField] string railgunReloadingSpeedCoeffName = "reloadingSpeedCoeff";

        [SerializeField] string railgunChargeSpeedCoeffName = "chargeSpeedCoeff";

        [SerializeField] int railgunReloadingCyclesCount = 2;

        [SerializeField] AnimationClip reloadClip;

        [SerializeField] AnimationClip chargeClip;

        Animator railgunAnimator;

        int railgunChargeSpeedCoeffID;

        int railgunChargingTriggerID;

        int railgunReloadingSpeedCoeffID;

        int railgunStartReloadingID;

        int railgunStopAnimationID;

        int railgunStopReloadingID;

        public void InitRailgunAnimation(Animator animator, float reloadingSpeed, float chargingTime) {
            railgunChargingTriggerID = Animator.StringToHash(railgunChargingTriggerName);
            railgunStartReloadingID = Animator.StringToHash(railgunStartReloadingName);
            railgunStopReloadingID = Animator.StringToHash(railgunStopReloadingName);
            railgunStopAnimationID = Animator.StringToHash(railgunStopAnimationName);
            railgunReloadingSpeedCoeffID = Animator.StringToHash(railgunReloadingSpeedCoeffName);
            railgunChargeSpeedCoeffID = Animator.StringToHash(railgunChargeSpeedCoeffName);
            railgunAnimator = animator;
            float length = reloadClip.length;
            float value = reloadingSpeed * railgunReloadingCyclesCount * length;
            float length2 = chargeClip.length;
            float value2 = length2 / chargingTime;
            animator.SetFloat(railgunReloadingSpeedCoeffID, value);
            animator.SetFloat(railgunChargeSpeedCoeffID, value2);
        }

        public void StartChargingAnimation() {
            railgunAnimator.ResetTrigger(railgunStopReloadingID);
            railgunAnimator.SetTrigger(railgunChargingTriggerID);
            railgunAnimator.Update(0f);
        }

        public void StopAnyRailgunAnimation() {
            railgunAnimator.SetTrigger(railgunStopAnimationID);
        }

        public void StartReloading() {
            railgunAnimator.ResetTrigger(railgunStopReloadingID);
            railgunAnimator.SetTrigger(railgunStartReloadingID);
        }

        public void StopReloading() {
            railgunAnimator.SetTrigger(railgunStopReloadingID);
        }
    }
}