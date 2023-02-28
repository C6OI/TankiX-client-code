using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientQuests.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class InBattleQuestItemGUIComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI taskText;

        [SerializeField] ImageSkin taskImageSkin;

        [SerializeField] TextMeshProUGUI currentProgressValue;

        [SerializeField] TextMeshProUGUI targetProgressValue;

        [SerializeField] Animator animator;

        [SerializeField] InBattleQuestItemGUIRewardContainerComponent rewardContainer;

        bool questCompleted;

        public string TaskText {
            get => taskText.text;
            set => taskText.text = value;
        }

        public string TargetProgressValue {
            get => targetProgressValue.text;
            set => targetProgressValue.text = value;
        }

        public string CurrentProgressValue {
            get => currentProgressValue.text;
            set => currentProgressValue.text = value;
        }

        public void SetReward(BattleQuestReward reward, int quatity, long itemId) {
            rewardContainer.SetActiveReward(reward, quatity, itemId);
        }

        public void SetImage(string spriteUid) {
            taskImageSkin.SpriteUid = spriteUid;
        }

        public void UpdateCurrentProgressValue(string newCurrentProgressValue, bool questCompleted = false) {
            this.questCompleted = questCompleted;
            CurrentProgressValue = newCurrentProgressValue;
            animator.SetTrigger("ShowProgress");
        }

        public void ProgressWasShown() {
            if (questCompleted) {
                CompleteQuest();
            }
        }

        public void DestroyQuest() {
            Destroy(gameObject);
        }

        public void CompleteQuest() {
            animator.SetTrigger("Complete");
        }
    }
}