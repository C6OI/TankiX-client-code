using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PremiumItemSystem : ECSSystem {
        [OnEventFire]
        public void OnAddPremiumBoostItem(NodeAddedEvent e, PremiumBoostItemNode item) { }

        [OnEventFire]
        public void OnAddPremiumQuestItem(NodeAddedEvent e, PremiumQuestItemNode item) { }

        [OnEventFire]
        public void OnAddPromoItem(NodeAddedEvent e, SingleNode<PremiumPromoComponent> item) { }

        [OnEventFire]
        public void OnAddPromoItem(NodeAddedEvent e, SingleNode<PremiumDurationChangedComponent> item) { }

        [OnEventFire]
        public void OnAddPromoItem(NodeRemoveEvent e, SingleNode<PremiumDurationChangedComponent> item) { }

        [OnEventFire]
        public void UserBoughtPremium(NodeAddedEvent e, SingleNode<PremiumWasBoughtComponent> user) { }

        public class PremiumBoostItemNode : Node {
            public DurationUserItemComponent durationUserItem;
            public PremiumBoostItemComponent premiumBoostItem;
        }

        public class PremiumQuestItemNode : Node {
            public DurationUserItemComponent durationUserItem;
            public PremiumQuestItemComponent premiumQuestItem;
        }
    }
}