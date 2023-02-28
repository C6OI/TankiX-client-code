using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class CircleProgressBar : MonoBehaviour {
        [SerializeField] float animationSpeed = 1f;

        [SerializeField] Image mainProgressImage;

        [SerializeField] Image additionalProgressImage;

        [SerializeField] Image additionalProgressContainer;

        [SerializeField] Image additionalProgressImage1;

        [SerializeField] Image additionalProgressContainer1;

        [SerializeField] float progress;

        [SerializeField] float additionalProgress;

        [SerializeField] float additionalProgress1;

        public Action allAnimationComplete;

        bool animated = true;

        List<ProgressBarUpgradeAnimation> upgradeAnimations = new();

        public float Progress {
            set {
                progress = value;
                additionalProgress = 0f;
                additionalProgress1 = 0f;
            }
        }

        public float AdditionalProgress {
            set => additionalProgress = value;
        }

        public float AdditionalProgress1 {
            set => additionalProgress1 = value;
        }

        void Update() {
            if (!animated) {
                return;
            }

            float num = Mathf.Abs(progress - mainProgressImage.fillAmount);

            if (num != 0f) {
                mainProgressImage.fillAmount = Mathf.Lerp(mainProgressImage.fillAmount, progress, Time.deltaTime * animationSpeed / num);
            }

            float num2 = Mathf.Abs(additionalProgress - additionalProgressContainer.fillAmount);

            if (num2 != 0f) {
                additionalProgressContainer.fillAmount = Mathf.Lerp(additionalProgressContainer.fillAmount, additionalProgress, Time.deltaTime * animationSpeed / num2);
            }

            float num3 = -360f * mainProgressImage.fillAmount;
            additionalProgressContainer.rectTransform.eulerAngles = new Vector3(0f, 0f, num3);
            additionalProgressImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, 0f - num3);
            float num4 = 0f;

            if (additionalProgressContainer1 != null && additionalProgressImage1 != null) {
                num4 = Mathf.Abs(additionalProgress1 - additionalProgressContainer1.fillAmount);

                if (num4 != 0f) {
                    additionalProgressContainer1.fillAmount = Mathf.Lerp(additionalProgressContainer1.fillAmount, additionalProgress1, Time.deltaTime * animationSpeed / num4);
                }

                num3 = -360f * mainProgressImage.fillAmount - 360f * additionalProgressContainer.fillAmount;
                additionalProgressContainer1.rectTransform.eulerAngles = new Vector3(0f, 0f, num3);
                additionalProgressImage1.rectTransform.localEulerAngles = new Vector3(0f, 0f, 0f - num3);
            }

            if (num == 0f && num2 == num && num4 == num) {
                SelectNextUpgradeAnimation();
            }
        }

        public void Animate(float delay) {
            CancelInvoke();
            Invoke("Animate", delay);
        }

        void Animate() {
            animated = true;
        }

        public void StopAnimation() {
            animated = false;
        }

        public void ResetProgressView() {
            mainProgressImage.fillAmount = 0f;
            additionalProgressContainer.fillAmount = 0f;
            additionalProgressContainer.rectTransform.eulerAngles = Vector3.zero;
            additionalProgressImage.rectTransform.localEulerAngles = Vector3.zero;
            additionalProgressContainer1.fillAmount = 0f;
            additionalProgressContainer1.rectTransform.eulerAngles = Vector3.zero;
            additionalProgressImage1.rectTransform.localEulerAngles = Vector3.zero;
        }

        public void ClearUpgradeAnimations() {
            upgradeAnimations = new List<ProgressBarUpgradeAnimation>();
        }

        public void AddUpgradeAnimation(float progress, float additionalProgress, float additionalProgress1 = 0f) {
            upgradeAnimations.Add(new ProgressBarUpgradeAnimation(progress, additionalProgress, additionalProgress1));
        }

        public void SelectNextUpgradeAnimation() {
            if (upgradeAnimations.Count > 0) {
                ProgressBarUpgradeAnimation progressBarUpgradeAnimation = upgradeAnimations[0];
                upgradeAnimations.Remove(progressBarUpgradeAnimation);
                ResetProgressView();
                Progress = progressBarUpgradeAnimation.progress;
                AdditionalProgress = progressBarUpgradeAnimation.additionalProgress;
                AdditionalProgress1 = progressBarUpgradeAnimation.additionalProgress1;
            } else if (allAnimationComplete != null) {
                allAnimationComplete();
                allAnimationComplete = null;
            }
        }
    }
}