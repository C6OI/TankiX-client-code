using System.Collections;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class GameplayChestResultUI : MonoBehaviour {
        [SerializeField] AnimatedDiffRadialProgress progress;

        [SerializeField] protected TextMeshProUGUI progressValue;

        [SerializeField] ImageSkin chestIcon;

        [SerializeField] Button openChestButton;

        [SerializeField] Animator animator;

        [SerializeField] TooltipShowBehaviour progressTooltip;

        [SerializeField] LocalizedField progressValueFormat;

        public long chestCount;

        [SerializeField] long previousScores;

        [SerializeField] long earnedScores;

        [SerializeField] long limitScores;

        float previousProgress;

        public void SetGameplayChestResult(string icon, long currentScores, long limitScores, long earnedScores) {
            this.earnedScores = earnedScores;
            this.limitScores = limitScores;
            progressValue.text = string.Format(progressValueFormat, earnedScores);
            chestIcon.SpriteUid = icon;
            long num = (currentScores - earnedScores) % limitScores;
            previousScores = num < 0 ? limitScores + num : num;
            previousProgress = Mathf.Clamp01(previousScores / (float)limitScores);
            progress.Set(previousProgress, previousProgress);
            progressTooltip.gameObject.SetActive(false);
            openChestButton.gameObject.SetActive(false);
        }

        public void ShowGameplayChestResult() {
            if (previousScores + earnedScores >= limitScores && chestCount > 1) {
                progress.Set(previousProgress, 1f);
                earnedScores -= limitScores - previousScores;
                previousScores = 0L;
                StartCoroutine(AnimateProgress());
            }

            if (previousScores + earnedScores >= limitScores && chestCount == 1) {
                progress.Set(previousProgress, 1f);
                animator.SetTrigger("GotChest");
                earnedScores -= limitScores - previousScores;
                previousScores = 0L;
                StartCoroutine(AnimateProgress());
            }

            if (previousScores + earnedScores < limitScores && chestCount < 1) {
                float newValue = Mathf.Clamp01((previousScores + earnedScores) / (float)limitScores);
                progress.Set(previousProgress, newValue);
                progressTooltip.gameObject.SetActive(true);
                progressTooltip.TipText = string.Format("{0} / {1}", previousScores + earnedScores, limitScores);
            }
        }

        IEnumerator AnimateProgress() {
            yield return new WaitForSeconds(0.3f);

            ResetProgress();
        }

        public void OpenGameplayChest() {
            animator.SetTrigger("ChestRewardTaken");
            previousProgress = 0f;
            float num = Mathf.Clamp01((previousScores + earnedScores) / (float)limitScores);
            progress.Set(num, num);
            chestCount = 0L;
            ShowGameplayChestResult();
        }

        public void ResetProgress() {
            previousProgress = 0f;
            progress.Set(0f, 0f);
            chestCount--;
            ShowGameplayChestResult();
        }
    }
}