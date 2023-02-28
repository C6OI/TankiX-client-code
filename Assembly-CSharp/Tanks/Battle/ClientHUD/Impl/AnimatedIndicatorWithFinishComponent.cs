using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NormalizedAnimatedValue))]
    public abstract class AnimatedIndicatorWithFinishComponent<T> : MonoBehaviour where T : Component, new() {
        bool animationFinished;
        Entity screenEntity;

        void OnEnable() {
            animationFinished = false;
        }

        protected void SetEntity(Entity screenEntity) {
            this.screenEntity = screenEntity;
        }

        void CheckIfAnimationFinished(float currentVal = 1f, float targetVal = 1f) {
            if (!animationFinished && MathUtil.NearlyEqual(currentVal, targetVal, 0.005f)) {
                SetAnimationFinished();
            }
        }

        void SetAnimationFinished() {
            animationFinished = true;
            screenEntity.AddComponent<T>();
        }

        protected void TryToSetAnimationFinished(float currentVal, float targetVal) {
            CheckIfAnimationFinished(currentVal, targetVal);
        }

        protected void TryToSetAnimationFinished() {
            if (!animationFinished) {
                SetAnimationFinished();
            }
        }
    }
}