using System.Collections.Generic;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemsScreenSystem : ECSSystem {
        [OnEventFire]
        public void ReplaceBoughtItem(NodeAddedEvent e, UserItemNode userItemNode,
            [JoinByMarketItem] MarketListItemNode boughtItem, [JoinAll] ScreenNode screenNode,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode, [JoinByScreen] ItemsListNode itemsList) {
            Entity entity = boughtItem.Entity;

            if (itemsList.itemsListForView.Items.Contains(entity)) {
                Entity entity2 = userItemNode.Entity;
                itemsList.itemsListForView.Items.Add(entity2);
                int index = screenNode.simpleHorizontalList.GetIndex(entity);
                screenNode.simpleHorizontalList.RemoveItem(entity);
                screenNode.simpleHorizontalList.AddItem(entity2);
                screenNode.simpleHorizontalList.SetIndex(entity2, index);

                if (selectedItemNode.component.SelectedItem == entity) {
                    ScheduleEvent<SelectGarageItemEvent>(entity2);
                }
            }
        }

        [OnEventComplete]
        public void MarkMountedItem(ShowGarageItemsEvent e, [Combine] MountedUserItemNode item,
            [JoinAll] ScreenNode screenNode, [JoinByScreen] ItemsListNode itemsList) =>
            MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, true);

        [OnEventFire]
        public void UnMarkMountedItem(NodeRemoveEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode,
            [JoinByScreen] ItemsListNode itemsList) =>
            MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, false);

        [OnEventFire]
        public void MarkMountedItem(NodeAddedEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode,
            [JoinByScreen] ItemsListNode itemsList) =>
            MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, true);

        [OnEventComplete]
        public void MoveToMountedItem(ShowGarageItemsEvent e, [Combine] SelectedMountedUserItemNode item,
            [JoinAll] ScreenNode screenNode) => screenNode.simpleHorizontalList.MoveToItem(item.Entity);

        void MarkItem(Entity itemEntity, List<Entity> itemsForView, ScreenNode screenNode, bool mark) {
            if (itemsForView.Contains(itemEntity)) {
                GameObject item = screenNode.simpleHorizontalList.GetItem(itemEntity);

                TickMarkerComponent componentInChildrenIncludeInactive =
                    item.GetComponentInChildrenIncludeInactive<TickMarkerComponent>();

                componentInChildrenIncludeInactive.gameObject.SetActive(mark);
            }
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public ScreenComponent screen;

            public SimpleHorizontalListComponent simpleHorizontalList;
        }

        public class UserItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public MarketItemGroupComponent marketItemGroup;

            public UserItemComponent userItem;
        }

        public class MarketListItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;
        }

        public class ItemsListNode : Node {
            public ItemsListForViewComponent itemsListForView;

            public ScreenGroupComponent screenGroup;
        }

        public class MountedUserItemNode : Node {
            public MountedItemComponent mountedItem;

            public UserItemComponent userItem;
        }

        public class SelectedMountedUserItemNode : Node {
            public MountedItemComponent mountedItem;

            public SelectedItemComponent selectedItem;

            public UserItemComponent userItem;
        }
    }
}