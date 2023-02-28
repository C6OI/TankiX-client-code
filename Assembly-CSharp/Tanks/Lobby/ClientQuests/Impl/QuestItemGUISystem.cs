using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientQuests.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestItemGUISystem : ECSSystem {
        [OnEventFire]
        public void SetTargetValue(NodeAddedEvent e, QuestGUINode quest) {
            quest.questProgressGUI.TargetProgressValue = quest.questProgress.TargetValue.ToString();
        }

        [OnEventFire]
        public void SetCurrentQuestProgress(NodeAddedEvent e, QuestWithoutProgressNode quest) {
            quest.questProgressGUI.Initialize(quest.questProgress.CurrentValue, quest.questProgress.TargetValue);
        }

        [OnEventFire]
        public void SetPreviousQuestProgress(NodeAddedEvent e, QuestWithProgressNode quest) {
            quest.questProgressGUI.Initialize(quest.questProgressAnimator.ProgressPrevValue, quest.questProgress.TargetValue);
        }

        [OnEventFire]
        public void SetQuestCompleted(NodeAddedEvent e, CompleteQuestNode quest) {
            if (quest.questProgress.PrevValue.Equals(quest.questProgress.CurrentValue)) {
                ShowCompletedQuest(quest);
            }
        }

        [OnEventFire]
        public void SetQuestRarity(NodeAddedEvent e, [Combine] QuestRarityNode quest, [JoinByUser] [Context] SelfPremiumQuestUserNode user) {
            if (quest.questRarity.RarityType.Equals(QuestRarityType.PREMIUM)) {
                Date endDate = user.premiumAccountQuest.EndDate;
                Date date = quest.questExpireDate.Date;
                int num = !(date > endDate) ? (int)((endDate - date) / 86400f) + 1 : 0;
                int num2 = !quest.Entity.HasComponent<CompleteQuestComponent>() ? 1 : 0;
                int count = num + num2;
                quest.questItemGUI.ShowPremiumBack(count);
            }
        }

        [OnEventFire]
        public void SetQuestChangeAbility(NodeAddedEvent e, QuestRarityNode quest, [JoinByUser] Optional<NotTakenQuestBonusNode> bonus,
            [JoinAll] SingleNode<QuestWindowComponent> questsScreen) {
            quest.questItemGUI.SetChangeButtonActivity(bonus.IsPresent() && !quest.questRarity.RarityType.Equals(QuestRarityType.PREMIUM) && questsScreen.component.ShowOnMainScreen);
        }

        [OnEventFire]
        public void SetQuestChangeAbility(NodeAddedEvent e, TakenQuestBonusNode bonus, [JoinByUser] [Combine] QuestRarityNode quest) {
            quest.questItemGUI.SetChangeButtonActivity(false);
        }

        [OnEventFire]
        public void HideQuestsChangeMenu(HideQuestsChangeMenuEvent e, DailyQuestGUINode openingQuest, [JoinByUser] [Combine] DailyQuestGUINode quest) {
            if (!quest.Entity.Id.Equals(openingQuest.Entity.Id)) {
                quest.questItemGUI.RejectChangeQuest();
            }
        }

        [OnEventFire]
        public void SetQuestDescription(NodeAddedEvent e, QuestDescriptionNode quest) {
            string descriptionPart = GetDescriptionPart(quest.questConditionDescription.condition.cases, quest.questProgress.TargetValue);
            quest.questItemGUI.TaskText = string.Format(quest.questConditionDescription.condition.format, quest.questProgress.TargetValue, descriptionPart);
        }

        [OnEventFire]
        public void SetKillsInOneBattleQuestConditionGUI(NodeAddedEvent e, KillsInOneBattleQuestNode quest) {
            QuestParameters questParameters = quest.questVariations.Quests.Find(r => IndexRange.ParseString(r.Range).Contains(quest.userRank.Rank));
            quest.questItemGUI.TaskText = string.Format(quest.questConditionDescription.condition.format, questParameters.TargetValue);
        }

        [OnEventFire]
        public void SetKillsInManyBattlesQuestConditionGUI(NodeAddedEvent e, KillsInManyBattlesQuestNode quest) {
            KillsInManyBattlesQuestComponent killsInManyBattlesQuest = quest.killsInManyBattlesQuest;
            QuestParameters questParameters = quest.questVariations.Quests.Find(r => IndexRange.ParseString(r.Range).Contains(quest.userRank.Rank));
            quest.questItemGUI.TaskText = string.Format(quest.questConditionDescription.condition.format, questParameters.TargetValue, killsInManyBattlesQuest.Battles);
        }

        [OnEventFire]
        public void SetKillsEveryDayQuestConditionGUI(NodeAddedEvent e, KillsInOneBattleEveryDayQuestNode quest) {
            KillsInOneBattleEveryDayQuestComponent killsInOneBattleEveryDayQuest = quest.killsInOneBattleEveryDayQuest;
            QuestParameters questParameters = quest.questVariations.Quests.Find(r => IndexRange.ParseString(r.Range).Contains(quest.userRank.Rank));
            quest.questItemGUI.TaskText = string.Format(quest.questConditionDescription.condition.format, questParameters.TargetValue, killsInOneBattleEveryDayQuest.Days);
        }

        string GetDescriptionPart(Dictionary<CaseType, string> cases, float value) {
            if (cases.Count == 0) {
                return string.Empty;
            }

            CaseType @case = CasesUtil.GetCase((int)value);
            return !cases.ContainsKey(@case) ? cases[CaseType.DEFAULT] : cases[@case];
        }

        [OnEventFire]
        public void SetQuestReward(NodeAddedEvent e, OldQuestGUINode quest) {
            QuestParameters questParameters = quest.questVariations.Quests.Find(r => IndexRange.ParseString(r.Range).Contains(quest.userRank.Rank));
            SetQuestReward(quest.questRewardGUI, questParameters.QuestReward);
        }

        [OnEventFire]
        public void SetQuestReward(NodeAddedEvent e, DailyQuestGUINode quest) {
            if (quest.questReward.Reward != null) {
                SetQuestReward(quest.questRewardGUI, quest.questReward.Reward);
            }
        }

        void SetQuestReward(QuestRewardGUIComponent questRewardGUI, Dictionary<long, int> reward) {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(reward.First().Key);

            if (reward.First().Value > 1) {
                questRewardGUI.RewardInfoText = reward.First().Value.ToString();
            } else {
                questRewardGUI.RewardInfoText = entity.GetComponent<DescriptionItemComponent>().Name;
            }

            string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;

            if (!string.IsNullOrEmpty(spriteUid)) {
                questRewardGUI.SetImage(spriteUid);
            }
        }

        [OnEventFire]
        public void SetDailyQuestConditions(NodeAddedEvent e, QuestConditionNode quest) {
            if (quest.questCondition.Condition != null && quest.questCondition.Condition.Count != 0) {
                QuestConditionType key = quest.questCondition.Condition.First().Key;
                long value = quest.questCondition.Condition.First().Value;
                string arg;

                if (key == QuestConditionType.MODE) {
                    arg = ((BattleMode)value).ToString();
                } else {
                    Entity entity = Flow.Current.EntityRegistry.GetEntity(value);
                    arg = entity.GetComponent<DescriptionItemComponent>().Name;
                }

                quest.questItemGUI.ConditionText = string.Format(quest.questConditionDescription.restrictionFormat, quest.questConditionDescription.restrictions[key], arg);
            }
        }

        [OnEventFire]
        public void ShowQuestProgressAnimation(ShowQuestGUIAnimationEvent e, QuestWithProgressNode quest) {
            quest.questProgressGUI.DeltaProgressValue = (quest.questProgress.CurrentValue - quest.questProgressAnimator.ProgressPrevValue).ToString();
            quest.questProgressGUI.Set(quest.questProgress.CurrentValue, quest.questProgress.TargetValue);
            quest.Entity.RemoveComponent<QuestProgressAnimatorComponent>();
        }

        [OnEventFire]
        public void SetQuestCompleted(ShowQuestGUIAnimationEvent e, CompleteQuestWithProgressNode quest) {
            NewEvent<ShowQuestCompleteEvent>().Attach(quest).ScheduleDelayed(e.ProgressDelay);
        }

        [OnEventFire]
        public void ShowQuestComplete(ShowQuestCompleteEvent e, QuestGUINode quest) {
            quest.questItemGUI.CompeleQuest();
        }

        void ShowCompletedQuest(QuestGUINode quest) {
            quest.questItemGUI.SetQuestCompleted(true);
        }

        [OnEventFire]
        public void ShowQuestExpireDate(NodeAddedEvent e, QuestExpireDateNode date) {
            date.questExpireTimer.EndDate = new Date(date.questExpireDate.Date.UnityTime);
        }

        [OnEventFire]
        public void MarkQuestResultAsResult(NodeAddedEvent e, QuestResultGUINode quest) {
            quest.questItemGUI.SetQuestResult(true);
        }

        public class SelfPremiumQuestUserNode : Node {
            public PremiumAccountQuestComponent premiumAccountQuest;
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class QuestNode : Node {
            public QuestComponent quest;

            public QuestProgressComponent questProgress;
        }

        public class DailyQuestNode : QuestNode {
            public QuestRewardComponent questReward;
        }

        public class QuestGUINode : QuestNode {
            public QuestItemGUIComponent questItemGUI;

            public QuestProgressGUIComponent questProgressGUI;

            public QuestRewardGUIComponent questRewardGUI;
        }

        public class DailyQuestGUINode : DailyQuestNode {
            public QuestItemGUIComponent questItemGUI;

            public QuestProgressGUIComponent questProgressGUI;

            public QuestRewardGUIComponent questRewardGUI;
        }

        public class QuestRarityNode : DailyQuestGUINode {
            public QuestExpireDateComponent questExpireDate;
            public QuestRarityComponent questRarity;
        }

        public class QuestDescriptionNode : DailyQuestGUINode {
            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class QuestConditionNode : DailyQuestGUINode {
            public QuestConditionComponent questCondition;

            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class QuestWithProgressNode : QuestGUINode {
            public QuestProgressAnimatorComponent questProgressAnimator;
        }

        [Not(typeof(QuestProgressAnimatorComponent))]
        public class QuestWithoutProgressNode : QuestGUINode { }

        public class CompleteQuestWithProgressNode : QuestWithProgressNode {
            public CompleteQuestComponent completeQuest;
        }

        public class CompleteQuestNode : QuestWithoutProgressNode {
            public CompleteQuestComponent completeQuest;
        }

        public class QuestImageNode : QuestGUINode {
            public ImageItemComponent imageItem;
        }

        public class OldQuestNode : QuestNode {
            public QuestVariationsComponent questVariations;
            public UserRankComponent userRank;
        }

        public class OldQuestGUINode : OldQuestNode {
            public QuestItemGUIComponent questItemGUI;

            public QuestProgressGUIComponent questProgressGUI;

            public QuestRewardGUIComponent questRewardGUI;
        }

        public class KillsInOneBattleQuestNode : OldQuestGUINode {
            public KillsInOneBattleQuestComponent killsInOneBattleQuest;

            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class KillsInManyBattlesQuestNode : OldQuestGUINode {
            public KillsInManyBattlesQuestComponent killsInManyBattlesQuest;

            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class KillsInOneBattleEveryDayQuestNode : OldQuestGUINode {
            public KillsInOneBattleEveryDayQuestComponent killsInOneBattleEveryDayQuest;

            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class QuestResultGUINode : QuestNode {
            public QuestItemGUIComponent questItemGUI;

            public QuestResultComponent questResult;
        }

        [Not(typeof(TakenBonusComponent))]
        public class NotTakenQuestBonusNode : Node {
            public QuestExchangeBonusComponent questExchangeBonus;
            public UserGroupComponent userGroup;
        }

        public class TakenQuestBonusNode : Node {
            public QuestExchangeBonusComponent questExchangeBonus;

            public TakenBonusComponent takenBonus;
            public UserGroupComponent userGroup;
        }

        public class ShowQuestCompleteEvent : Event { }

        public class QuestExpireDateNode : Node {
            public QuestExpireDateComponent questExpireDate;

            public QuestExpireTimerComponent questExpireTimer;
        }
    }
}