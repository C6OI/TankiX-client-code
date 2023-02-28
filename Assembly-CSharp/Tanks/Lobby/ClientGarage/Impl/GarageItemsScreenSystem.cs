using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemsScreenSystem : ECSSystem {
        [OnEventFire]
        public void ReplaceBoughtItem(NodeAddedEvent e, UserItemNode userItemNode, [JoinByMarketItem] MarketItemNode boughtItem, [JoinAll] ScreenNode screenNode,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode, [JoinByScreen] ItemsListNode itemsList) {
            Entity entity = boughtItem.Entity;

            if (itemsList.itemsListForView.Items.Contains(entity)) {
                bool select = selectedItemNode.component.SelectedItem == entity;
                ReplaceItem(screenNode.simpleHorizontalList, itemsList.itemsListForView, entity, userItemNode.Entity, select);
            }
        }

        void ReplaceItem(SimpleHorizontalListComponent horizontalList, ItemsListForViewComponent itemsListForView, Entity origEntity, Entity newEntity, bool select) {
            int index = horizontalList.GetIndex(origEntity);
            horizontalList.RemoveItem(origEntity);
            itemsListForView.Items.Remove(origEntity);
            horizontalList.AddItem(newEntity);
            itemsListForView.Items.Add(newEntity);
            horizontalList.SetIndex(newEntity, index);

            if (select) {
                ScheduleEvent<SelectGarageItemEvent>(newEntity);
            }
        }

        [OnEventFire]
        public void ReplaceUserItemToMarketItem(NodeRemoveEvent e, UserItemNode userItem, [JoinByMarketItem] Optional<MarketItemNode> marketItem, [JoinAll] ScreenNode screenNode,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode, [JoinByScreen] ItemsListNode itemsList) {
            bool flag = selectedItemNode.component.SelectedItem == userItem.Entity;

            if (marketItem.IsPresent()) {
                ReplaceItem(screenNode.simpleHorizontalList, itemsList.itemsListForView, userItem.Entity, marketItem.Get().Entity, flag);
                return;
            }

            screenNode.simpleHorizontalList.RemoveItem(userItem.Entity);
            itemsList.itemsListForView.Items.Remove(userItem.Entity);

            if (flag && screenNode.simpleHorizontalList.Count != 0) {
                ScheduleEvent<SelectGarageItemEvent>(screenNode.simpleHorizontalList.GetItems().First());
            } else if (screenNode.simpleHorizontalList.Count == 0) {
                ScheduleEvent<ItemsListEmptyEvent>(screenNode);
            }
        }

        [OnEventComplete]
        public void MarkMountedItem(ShowGarageItemsEvent e, [Combine] MountedUserItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] ItemsListNode itemsList) {
            MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, true);
        }

        [OnEventFire]
        public void UnMarkMountedItem(NodeRemoveEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] ItemsListNode itemsList) {
            MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, false);
        }

        [OnEventFire]
        public void MarkMountedItem(NodeAddedEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] ItemsListNode itemsList) {
            MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, true);
        }

        [OnEventComplete]
        public void MoveToMountedItem(ShowGarageItemsEvent e, [Combine] SelectedMountedUserItemNode item, [JoinAll] ScreenNode screenNode) {
            screenNode.simpleHorizontalList.MoveToItem(item.Entity);
        }

        void MarkItem(Entity itemEntity, List<Entity> itemsForView, ScreenNode screenNode, bool mark) {
            if (itemsForView.Contains(itemEntity)) {
                GameObject item = screenNode.simpleHorizontalList.GetItem(itemEntity);
                TickMarkerComponent componentInChildrenIncludeInactive = item.GetComponentInChildrenIncludeInactive<TickMarkerComponent>();
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

        public class MarketItemNode : Node {
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