using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientProfile.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ProgressResultUI : MonoBehaviour {
        [SerializeField] AnimatedDiffRadialProgress experienceProgress;

        [SerializeField] protected TextMeshProUGUI expRewardUI;

        [SerializeField] Animator animator;

        protected LevelInfo currentLevelInfo;

        bool isLevelUp;

        protected int nextLevel;

        protected LevelInfo previousLevelInfo;

        float previousProgress;

        protected void SetProgress(float expReward, int[] levels, LevelInfo currentLevelInfo, BattleResultsTextTemplatesComponent textTemplates) {
            this.currentLevelInfo = currentLevelInfo;
            previousLevelInfo = LevelInfo.Get((long)(currentLevelInfo.AbsolutExperience - expReward), levels);

            if (previousLevelInfo.IsMaxLevel) {
                experienceProgress.Set(1f, 1f);
                expRewardUI.text = string.Empty;
                return;
            }

            previousProgress = Mathf.Clamp01(previousLevelInfo.Experience / (float)previousLevelInfo.MaxExperience);
            experienceProgress.Set(previousProgress, previousProgress);
            isLevelUp = currentLevelInfo.Level > previousLevelInfo.Level;
            nextLevel = previousLevelInfo.Level + 1;
            expRewardUI.text = string.Format(textTemplates.EarnedExperienceTextTemplate, expReward);
        }

        public void SetNewProgress() {
            if (!previousLevelInfo.IsMaxLevel) {
                if (isLevelUp) {
                    experienceProgress.Set(previousProgress, 1f);
                    animator.SetTrigger("LevelUp");
                } else {
                    float newValue = Mathf.Clamp01(currentLevelInfo.Experience / (float)currentLevelInfo.MaxExperience);
                    experienceProgress.Set(previousProgress, newValue);
                }
            }
        }

        protected void SetResidualProgress() {
            if (!currentLevelInfo.IsMaxLevel) {
                experienceProgress.Set(0f, 0f);

                if (nextLevel < currentLevelInfo.Level) {
                    experienceProgress.Set(0f, 1f);
                    animator.SetTrigger("LevelUp");
                    nextLevel++;
                } else {
                    float newValue = Mathf.Clamp01(currentLevelInfo.Experience / (float)currentLevelInfo.MaxExperience);
                    experienceProgress.Set(0f, newValue);
                }
            }
        }
    }
}