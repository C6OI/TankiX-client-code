using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestItemGUIComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener {
        [SerializeField] TextMeshProUGUI taskText;

        [SerializeField] TextMeshProUGUI conditionText;

        [SerializeField] QuestProgressGUIComponent questProgressGUIComponent;

        [SerializeField] QuestRewardGUIComponent questRewardGUIComponent;

        [SerializeField] Animator animator;

        [SerializeField] GameObject premiumBackground;

        [SerializeField] TextMeshProUGUI questsCount;

        [SerializeField] GameObject changeButton;

        Entity entity;

        public QuestProgressGUIComponent QuestProgressGUIComponent => questProgressGUIComponent;

        public QuestRewardGUIComponent QuestRewardGUIComponent => questRewardGUIComponent;

        public string ConditionText {
            get => conditionText.text;
            set {
                conditionText.text = value;
                conditionText.gameObject.SetActive(true);
            }
        }

        public string TaskText {
            get => taskText.text;
            set => taskText.text = value;
        }

        void AttachToEntityListener.AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity) {
            this.entity = null;
            Destroy(gameObject);
        }

        public void ShowQuest() {
            animator.SetTrigger("ActivateQuest");
        }

        public void RemoveQuest() {
            animator.SetTrigger("RemoveQuest");
        }

        public void AddQuest() {
            animator.SetTrigger("AddQuest");
        }

        public void CompeleQuest() {
            animator.SetTrigger("CompleteQuest");
        }

        public void SetQuestCompleted(bool value) {
            animator.SetBool("completedQuest", value);
        }

        public void SetQuestResult(bool value) {
            animator.SetBool("questResult", value);
        }

        public void ShowPremiumBack(int count) {
            premiumBackground.SetActive(true);
            questsCount.text = count.ToString();
        }

        public void SetChangeButtonActivity(bool active) {
            changeButton.SetActive(active);
        }

        void QuestRemoved() {
            if (entity != null) {
                EngineService.Engine.ScheduleEvent(new QuestRemovedEvent(), entity);
            } else {
                Destroy(gameObject);
            }
        }

        public void ChangeQuest() {
            if (entity != null) {
                animator.SetBool("showConfirmChangeQuest", true);
                EngineService.Engine.ScheduleEvent(new HideQuestsChangeMenuEvent(), entity);
            }
        }

        public void ConfirmChangeQuest() {
            if (entity != null) {
                EngineService.Engine.ScheduleEvent(new ChangeQuestEvent(), entity);
            }
        }

        public void RejectChangeQuest() {
            animator.SetBool("showConfirmChangeQuest", false);
        }
    }
}