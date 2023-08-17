using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public abstract class AbstractShotAnimationComponent : MonoBehaviour, Component {
        float CalculateEnergyAfterShotsWithCooldown(float eCool, float eShot) => Mathf.Clamp01(1f + eCool - eShot);

        float CalculateRequiredEnergyForNextShot(float currentEnergy, float eShot) => Mathf.Clamp01(eShot - currentEnergy);

        protected float CalculateOptimalAnimationTime(float energyReloadingSpeed, float cooldownTimeSec, float eShot) {
            float num = 0f;
            float num2 = energyReloadingSpeed * cooldownTimeSec;

            if (num2 >= eShot) {
                return cooldownTimeSec;
            }

            float currentEnergy = CalculateEnergyAfterShotsWithCooldown(num2, eShot);
            float num3 = CalculateRequiredEnergyForNextShot(currentEnergy, eShot);

            if (num3 == 0f) {
                return cooldownTimeSec;
            }

            return cooldownTimeSec + num3 / energyReloadingSpeed;
        }

        protected float CalculateShotSpeedCoeff(AnimationClip shotAnimationClip, float optimalAnimationTime,
            bool canPlaySlower) {
            float length = shotAnimationClip.length;
            float result = 1f;

            if (length > optimalAnimationTime) {
                result = length / optimalAnimationTime;
            } else if (length < optimalAnimationTime && canPlaySlower) {
                result = length / optimalAnimationTime;
            }

            return result;
        }
    }
}