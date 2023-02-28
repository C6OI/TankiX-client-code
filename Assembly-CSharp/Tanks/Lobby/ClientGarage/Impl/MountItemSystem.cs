using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MountItemSystem : ECSSystem {
        [OnEventFire]
        public void MountItem(ButtonClickEvent e, SingleNode<MountItemButtonComponent> button, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem) {
            ScheduleEvent<MountItemEvent>(selectedItem.component.SelectedItem);
            ScheduleEvent<MountParentItemEvent>(selectedItem.component.SelectedItem);
        }

        [OnEventFire]
        public void MountParentItem(MountParentItemEvent e, SkinUserItemNode skinUserItem, [JoinByParentGroup] [Combine] NotMountedUserItemNode parentUserItem,
            [JoinByMarketItem] MarketItemNode parentMarketItemNode) {
            if (parentMarketItemNode.Entity.Id == skinUserItem.parentGroup.Key) {
                ScheduleEvent<MountItemEvent>(parentUserItem);
            }
        }

        [OnEventFire]
        public void Crutch(MountItemEvent e, Node any) { }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;
        }

        public class UserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public ParentGroupComponent parentGroup;
            public UserItemComponent userItem;
        }

        [Not(typeof(MountedItemComponent))]
        public class NotMountedUserItemNode : UserItemNode { }

        public class SkinUserItemNode : UserItemNode {
            public SkinItemComponent skinItem;
        }

        public class MountParentItemEvent : Event { }
    }
}