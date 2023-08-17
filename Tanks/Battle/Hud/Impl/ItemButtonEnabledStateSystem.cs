using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonEnabledStateSystem : ECSSystem {
        [OnEventFire]
        public void PlayMineActivationAnimation(SupplyActivationRequestEvent e, MineEnabledStateNode mine,
            [JoinBySupply] ItemButtonNode button) {
            Animator animator = button.itemButton.Animator;
            ItemButtonAnimationUtils.ActivateMineButtonFromInventory(animator);
        }

        [OnEventFire]
        public void EnterEnabledState(NodeAddedEvent e, ItemButtonEnabledNode button,
            [Context] [JoinBySupply] ItemNode item) {
            Animator animator = button.itemButton.Animator;
            ItemButtonAnimationUtils.EnableItemButton(animator);
        }

        [OnEventFire]
        public void DeactivateMineButton(NodeRemoveEvent e, SingleNode<MineActivationEnabledComponent> inventory,
            [JoinBySupply] ItemButtonNode button) {
            if (inventory.Entity.HasComponent<InventoryItemEnabledStateComponent>()) {
                button.itemButtonESM.Esm.ChangeState<ItemButtonStates.ItemButtonDisabledStateNode>();
            }
        }

        [OnEventFire]
        public void ActivateButton(NodeAddedEvent e, ItemNode item, [Context] [JoinBySupply] ItemButtonNode button) {
            if (!item.Entity.HasComponent<MineInventoryComponent>()) {
                button.itemButtonESM.Esm.ChangeState<ItemButtonStates.ItemButtonEnabledStateNode>();
            }
        }

        [OnEventFire]
        public void ActivateMineButton(NodeAddedEvent e, MineActivationEnabledNode inventory,
            [Context] [JoinBySupply] ItemButtonNode button) =>
            button.itemButtonESM.Esm.ChangeState<ItemButtonStates.ItemButtonEnabledStateNode>();

        [OnEventFire]
        public void ActivateMineButton(NodeAddedEvent e, MineItemNode mine, [JoinBySupply] [Context] ItemButtonNode button) {
            if (!mine.Entity.HasComponent<MineActivationEnabledComponent>()) {
                button.itemButtonESM.Esm.ChangeState<ItemButtonStates.ItemButtonDisabledStateNode>();
            }
        }

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ItemButtonESMComponent itemButtonESM;

            public SupplyGroupComponent supplyGroup;
        }

        public class ItemButtonEnabledNode : Node {
            public ItemButtonComponent itemButton;

            public ItemButtonEnabledStateComponent itemButtonEnabledState;

            public ItemButtonESMComponent itemButtonESM;

            public SupplyGroupComponent supplyGroup;
        }

        public class ItemNode : Node {
            public InventoryItemEnabledStateComponent inventoryItemEnabledState;
            public SupplyBattleItemComponent supplyBattleItem;

            public SupplyGroupComponent supplyGroup;
        }

        public class MineItemNode : Node {
            public InventoryItemEnabledStateComponent inventoryItemEnabledState;

            public MineInventoryComponent mineInventory;
            public SupplyBattleItemComponent supplyBattleItem;

            public SupplyGroupComponent supplyGroup;
        }

        public class MineActivationEnabledNode : Node {
            public InventoryItemEnabledStateComponent inventoryItemEnabledState;

            public MineActivationEnabledComponent mineActivationEnabled;
            public MineInventoryComponent mineInventory;
        }

        public class MineEnabledStateNode : Node {
            public InventoryItemEnabledStateComponent inventoryItemEnabledState;
            public MineInventoryComponent mineInventory;
        }
    }
}