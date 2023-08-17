using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class VulcanTurbineAnimationComponent : MonoBehaviour, Component {
        [SerializeField] string turbineStateName = "TurbineRotation";

        [SerializeField] int turbineLayerIndex;

        [SerializeField] string turbineCoeffName = "turbineCoeff";

        [SerializeField] string turbineStopName = "stopTurbine";

        [SerializeField] int turbineMaxSpeedCoeff = 1;

        float currentAcceleration;

        float currentNrmTime;

        float currentSpeed;

        bool isRunningTurbine;

        float slowDownAcceleration;

        float speedUpAcceleration;

        int turbineCoeffID;

        int turbineStateID;

        int turbineStopID;

        Animator vulcanAnimator;

        void Awake() => enabled = false;

        void Update() {
            currentSpeed += currentAcceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, turbineMaxSpeedCoeff);
            vulcanAnimator.SetFloat(turbineCoeffID, currentSpeed);

            if (currentSpeed <= 0f) {
                StopTurbine();
            }
        }

        public void Init(Animator animator, float speedUpTime, float slowDownTime) {
            vulcanAnimator = animator;
            turbineStateID = Animator.StringToHash(turbineStateName);
            turbineCoeffID = Animator.StringToHash(turbineCoeffName);
            turbineStopID = Animator.StringToHash(turbineStopName);
            speedUpAcceleration = turbineMaxSpeedCoeff / speedUpTime;
            slowDownAcceleration = -turbineMaxSpeedCoeff / slowDownTime;
            currentNrmTime = 0f;
            currentSpeed = 0f;
            vulcanAnimator.SetFloat(turbineCoeffID, currentSpeed);
            isRunningTurbine = false;
        }

        public void StopTurbine() {
            currentSpeed = 0f;
            currentNrmTime = Mathf.Repeat(vulcanAnimator.GetCurrentAnimatorStateInfo(turbineLayerIndex).normalizedTime, 1f);
            vulcanAnimator.SetTrigger(turbineStopID);
            isRunningTurbine = false;
            enabled = false;
        }

        public void StartSpeedUp() => StartChangeSpeedPhase(true);

        public void StartSlowDown() => StartChangeSpeedPhase(false);

        public void StartShooting() {
            PlayAnimatorIfNeed();
            currentSpeed = turbineMaxSpeedCoeff;
            vulcanAnimator.SetFloat(turbineCoeffID, currentSpeed);
            enabled = false;
            isRunningTurbine = true;
        }

        void StartChangeSpeedPhase(bool isSpeedUp) {
            currentAcceleration = !isSpeedUp ? slowDownAcceleration : speedUpAcceleration;
            PlayAnimatorIfNeed();
            enabled = true;
            isRunningTurbine = true;
        }

        void PlayAnimatorIfNeed() {
            if (!isRunningTurbine) {
                vulcanAnimator.Play(turbineStateID, turbineLayerIndex, currentNrmTime);
            }
        }
    }
}