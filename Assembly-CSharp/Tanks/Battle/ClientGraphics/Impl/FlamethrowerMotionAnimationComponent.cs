using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class FlamethrowerMotionAnimationComponent : MonoBehaviour, Component {
        [SerializeField] string motionCoeffName = "motionCoeff";

        [SerializeField] string stopMotionName = "stopMotion";

        [SerializeField] float idleMotionCoeff = -0.25f;

        [SerializeField] float workingMotionCoeff = 1f;

        [SerializeField] string motionStateName = "Motion";

        [SerializeField] int motionStateLayerIndex;

        float currentNrmTime;

        Animator flamethrowerAnimator;

        bool isWorkingMotion;

        int motionCoeffID;

        int motionStateID;

        int stopMotionID;

        void Awake() {
            enabled = false;
        }

        void Update() {
            float num = currentNrmTime;
            float normalizedTime = flamethrowerAnimator.GetCurrentAnimatorStateInfo(motionStateLayerIndex).normalizedTime;
            float num2 = normalizedTime - num;
            currentNrmTime += num2;
            currentNrmTime = Mathf.Repeat(currentNrmTime, 1f);
        }

        void StartMotion(bool isWorking) {
            isWorkingMotion = isWorking;
            flamethrowerAnimator.ResetTrigger(stopMotionID);
            flamethrowerAnimator.SetFloat(motionCoeffID, !isWorkingMotion ? idleMotionCoeff : workingMotionCoeff);
            flamethrowerAnimator.Play(motionStateID, motionStateLayerIndex, currentNrmTime);
            enabled = true;
        }

        public void Init(Animator animator) {
            flamethrowerAnimator = animator;
            currentNrmTime = 0f;
            motionCoeffID = Animator.StringToHash(motionCoeffName);
            stopMotionID = Animator.StringToHash(stopMotionName);
            motionStateID = Animator.StringToHash(motionStateName);
        }

        public void StartWorkingMotion() {
            StartMotion(true);
        }

        public void StartIdleMotion() {
            StartMotion(false);
        }

        public void StopMotion() {
            flamethrowerAnimator.SetTrigger(stopMotionID);
            enabled = false;
        }
    }
}