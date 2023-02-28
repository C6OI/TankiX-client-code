using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class HiddenInGarageItemSystem : ECSSystem {
        [OnEventFire]
        public void HideExclusiveItem(NodeAddedEvent e, [Combine] NotHiddenExclusiveMarketItemNode marketItem, SelfUserNode selfUser) {
            if (marketItem.exclusiveItem.ForbiddenForPublishers.Contains(selfUser.userPublisher.Publisher)) {
                marketItem.Entity.AddComponent<HiddenInGarageItemComponent>();
            }
        }

        [OnEventFire]
        public void ShowNotExclusiveItem(MarketItemAvailabilityUpdatedEvent e, HiddenExclusiveMarketItemNode marketItem, [JoinAll] SelfUserNode selfUser) {
            Debug.Log(string.Concat("HiddenInGarageItemSystem.ShowNotExclusiveItem ", marketItem.Entity, " ",
                !marketItem.exclusiveItem.ForbiddenForPublishers.Contains(selfUser.userPublisher.Publisher)));

            if (!marketItem.exclusiveItem.ForbiddenForPublishers.Contains(selfUser.userPublisher.Publisher)) {
                marketItem.Entity.RemoveComponent<HiddenInGarageItemComponent>();
            }
        }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;
        }

        public class ExclusiveMarketItemNode : MarketItemNode {
            public ExclusiveItemComponent exclusiveItem;
        }

        [Not(typeof(HiddenInGarageItemComponent))]
        public class NotHiddenExclusiveMarketItemNode : ExclusiveMarketItemNode { }

        public class HiddenExclusiveMarketItemNode : ExclusiveMarketItemNode {
            public HiddenInGarageItemComponent hiddenInGarageItem;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserPublisherComponent userPublisher;
        }
    }
}