using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MarketInfoSystem : ECSSystem {
        [OnEventFire]
        public void CreateSelf(NodeAddedEvent e, SingleNode<SelfUserComponent> selfUser) {
            selfUser.Entity.AddComponent(new PurchasedItemListComponent());
        }

        [OnEventFire]
        public void AddItem(NodeAddedEvent e, UserItemNode userItem, [JoinByUser] SelfUserNode selfUserNode) {
            selfUserNode.purchasedItemList.AddPurchasedItem(userItem.marketItemGroup.Key);
        }

        [OnEventFire]
        public void CheckItem(ItemInMarketRequestEvent e, Node any, [JoinAll] SelfUserNode selfUserNode, [JoinAll] [Combine] MarketItemNode item) {
            if (!selfUserNode.purchasedItemList.Contains(item.Entity.Id)) {
                e.marketItems.Add(item.Entity.Id, item.descriptionItem.Name);
            }
        }

        public class UserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public UserGroupComponent userGroup;

            public UserItemComponent userItem;
        }

        public class MarketItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public MarketItemComponent marketItem;
        }

        public class SelfUserNode : Node {
            public PurchasedItemListComponent purchasedItemList;
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }
    }
}