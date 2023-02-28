using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientQuests.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestOrderSystem : ECSSystem {
        [OnEventFire]
        public void SetQuestOrder(NodeAddedEvent e, OldQuestNode quest) {
            quest.Entity.AddComponent(new QuestOrderComponent {
                Index = quest.orderItem.Index
            });
        }

        [OnEventFire]
        public void SetQuestOrder(NodeAddedEvent e, NewQuestNode quest) {
            quest.Entity.AddComponent(new QuestOrderComponent {
                Index = quest.slotIndex.Index
            });
        }

        public class QuestNode : Node {
            public QuestComponent quest;

            public QuestProgressComponent questProgress;
        }

        public class OldQuestNode : QuestNode {
            public OrderItemComponent orderItem;
        }

        public class NewQuestNode : QuestNode {
            public SlotIndexComponent slotIndex;
        }
    }
}