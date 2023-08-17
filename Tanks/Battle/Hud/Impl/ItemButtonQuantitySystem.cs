using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonQuantitySystem : ECSSystem {
        [OnEventFire]
        public void UpdateItemCount(NodeAddedEvent e, ItemButtonNode itemButton, [Context] [JoinBySupply] ItemNode item,
            [JoinBySupply] SingleNode<SupplyCountComponent> userSupply) =>
            UpdateItemCount(itemButton, (int)userSupply.component.Count);

        static void UpdateItemCount(ItemButtonNode itemButton, int count) =>
            itemButton.itemButton.ValueText.text = count.ToString();

        [OnEventComplete]
        public void UpdateItemCount(InventoryItemCountUpdatedEvent e, ItemNode item,
            [JoinBySupply] ItemButtonNode itemButton, ItemNode item2,
            [JoinBySupply] SingleNode<SupplyCountComponent> userSupply) {
            userSupply.component.Count = e.Count;
            UpdateItemCount(itemButton, (int)e.Count);
        }

        [OnEventFire]
        public void ShowQuantityOnTankActiveStateExit(NodeAddedEvent e, SelfSemiActiveTankNode tank,
            [Combine] SingleNode<ItemButtonComponent> itemButton) {
            Animator animator = itemButton.component.Animator;
            ItemButtonAnimationUtils.ShowQuantityOnItemButton(animator);
        }

        [OnEventFire]
        public void HideQuantityOnTankActiveStateEnter(NodeRemoveEvent e, SelfSemiActiveTankNode tank,
            [Combine] SingleNode<ItemButtonComponent> itemButton) {
            Animator animator = itemButton.component.Animator;
            ItemButtonAnimationUtils.HideQuantityOnItemButton(animator);
        }

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public SupplyGroupComponent supplyGroup;
        }

        public class ItemNode : Node {
            public SupplyBattleItemComponent supplyBattleItem;

            public SupplyGroupComponent supplyGroup;
        }

        public class SelfSemiActiveTankNode : Node {
            public SelfTankComponent selfTank;
            public TankSemiActiveStateComponent tankSemiActiveState;
        }
    }
}