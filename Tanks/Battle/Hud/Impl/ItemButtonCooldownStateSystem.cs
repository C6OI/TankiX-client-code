using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonCooldownStateSystem : ECSSystem {
        [OnEventFire]
        public void InitializeCooldownFill(NodeAddedEvent e, ItemCooldownStateNode item,
            [Context] [JoinBySupply] ItemButtonNode itemButton) => StartCooldown(item, itemButton);

        [OnEventFire]
        public void InitializeCooldownFill(CooldownMergedEvent e, ItemCooldownStateNode item,
            [JoinBySupply] ItemButtonNode itemButton) => StartCooldown(item, itemButton);

        void StartCooldown(ItemCooldownStateNode item, ItemButtonNode itemButton) {
            itemButton.itemButtonESM.Esm.ChangeState<ItemButtonStates.ItemButtonCooldownStateNode>();
            InventoryItemCooldownTimeComponent inventoryItemCooldownTime = item.inventoryItemCooldownTime;

            float cooldownTime = inventoryItemCooldownTime.CooldownTime / 1000f +
                                 inventoryItemCooldownTime.CooldownStartTime.UnityTime -
                                 Date.Now.UnityTime;

            Animator animator = itemButton.itemButton.Animator;
            ItemButtonAnimationUtils.SwitchItemButtonToCooldownState(animator, cooldownTime);
        }

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ItemButtonESMComponent itemButtonESM;

            public SupplyGroupComponent supplyGroup;
        }

        public class ItemCooldownStateNode : Node {
            public InventoryItemCooldownStateComponent inventoryItemCooldownState;

            public InventoryItemCooldownTimeComponent inventoryItemCooldownTime;
            public SupplyBattleItemComponent supplyBattleItem;

            public SupplyGroupComponent supplyGroup;
        }
    }
}