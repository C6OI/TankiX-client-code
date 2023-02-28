using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoldBonusCounterSystem : ECSSystem {
        [OnEventFire]
        public void SetCounter(NodeAddedEvent e, SingleNode<GoldBonusCounterComponent> counter, SelfUserNode user, [JoinByUser] GoldBonusUserItemNode goldBonusItem) {
            counter.component.SetCount(goldBonusItem.userItemCounter.Count);
        }

        [OnEventFire]
        public void SetCounter(GoldBonusesCountChangedEvent e, SelfUserNode user, [JoinByUser] GoldBonusUserItemNode goldBonusItem,
            [JoinAll] SingleNode<GoldBonusCounterComponent> counter) {
            counter.component.SetCount(e.NewCount);
        }

        [OnEventFire]
        public void SetCounter(ItemsCountChangedEvent e, GoldBonusUserItemNode goldBonusItem, [JoinAll] SingleNode<GoldBonusCounterComponent> counter) {
            counter.component.SetCount(goldBonusItem.userItemCounter.Count);
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class GoldBonusUserItemNode : Node {
            public GoldBonusItemComponent goldBonusItem;

            public UserGroupComponent userGroup;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }
    }
}