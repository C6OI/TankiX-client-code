using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientQuests.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestsButtonNotificationBadgeSystem : ECSSystem {
        [OnEventFire]
        public void ShowAttentionBadge(NodeAddedEvent e, QuestsButtonNode questsButtonNode, [Combine] CompleteQuestNode quest) {
            if (!quest.Entity.HasComponent<RewardedQuestComponent>()) {
                questsButtonNode.notificationBadge.BadgeActivity = true;
            }
        }

        [OnEventFire]
        public void HideAttentionBadge(NodeRemoveEvent e, QuestsButtonNode questsButtonNode) {
            questsButtonNode.notificationBadge.BadgeActivity = false;
        }

        [OnEventFire]
        public void UpdateButton(NodeRemoveEvent e, SingleNode<RewardedQuestComponent> quest, [JoinAll] QuestsButtonNode button, [JoinAll] ICollection<CompleteQuestNode> quests) {
            foreach (CompleteQuestNode quest2 in quests) {
                if (quest2.Entity == quest.Entity || !quest2.Entity.HasComponent<RewardedQuestComponent>()) {
                    continue;
                }

                return;
            }

            button.notificationBadge.BadgeActivity = false;
        }

        public class CompleteQuestNode : Node {
            public CompleteQuestComponent completeQuest;
            public QuestComponent quest;
        }

        public class QuestsButtonNode : Node {
            public NotificationBadgeComponent notificationBadge;
            public QuestsButtonComponent questsButton;
        }
    }
}