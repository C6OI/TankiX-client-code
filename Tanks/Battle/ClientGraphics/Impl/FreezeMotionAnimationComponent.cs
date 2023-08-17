using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class FreezeMotionAnimationComponent : MonoBehaviour, Component {
        [SerializeField] float idleAnimationSpeedMultiplier = 0.125f;

        [SerializeField] float workingAnimationSpeedMultiplier = 1f;

        [SerializeField] string freezeAnimationSpeedCoeffName = "motionCoeff";

        [SerializeField] float coefAcceleration = 0.6f;

        float coeffChangeSpeed;

        float currentSpeedMultiplier;

        Animator freezeAnimator;

        float freezeEnergyReloadingSpeed;

        bool isWorking;

        int speedCoeffID;

        void Awake() => enabled = false;

        void Update() {
            currentSpeedMultiplier += coeffChangeSpeed * Time.deltaTime;

            if (currentSpeedMultiplier >= workingAnimationSpeedMultiplier) {
                currentSpeedMultiplier = workingAnimationSpeedMultiplier;

                if (isWorking) {
                    enabled = false;
                }
            }

            if (currentSpeedMultiplier <= idleAnimationSpeedMultiplier) {
                currentSpeedMultiplier = idleAnimationSpeedMultiplier;

                if (!isWorking) {
                    enabled = false;
                }
            }

            freezeAnimator.SetFloat(speedCoeffID, currentSpeedMultiplier);
        }

        void StartMode(float currentEnergyLevel, bool isWorking) {
            float num = 1f - currentEnergyLevel;

            coeffChangeSpeed =
                !isWorking ? 0f -
                             Mathf.Max(0f,
                                 (currentSpeedMultiplier - idleAnimationSpeedMultiplier) / num * freezeEnergyReloadingSpeed)
                    : coefAcceleration;

            enabled = true;
            this.isWorking = isWorking;
        }

        public void Init(Animator animator, float energyReloadingSpeed) {
            freezeAnimator = animator;
            freezeEnergyReloadingSpeed = energyReloadingSpeed;
            currentSpeedMultiplier = 0f;
            speedCoeffID = Animator.StringToHash(freezeAnimationSpeedCoeffName);
            freezeAnimator.SetFloat(speedCoeffID, currentSpeedMultiplier);
            isWorking = false;
        }

        public void StartWorking(float currentEnergyLevel) => StartMode(currentEnergyLevel, true);

        public void StartIdle(float currentEnergyLevel) => StartMode(currentEnergyLevel, false);

        public void StopMotion() => freezeAnimator.SetFloat(speedCoeffID, 0f);
    }
}