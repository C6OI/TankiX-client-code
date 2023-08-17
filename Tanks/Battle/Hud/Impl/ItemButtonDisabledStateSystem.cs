using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonDisabledStateSystem : ECSSystem {
        [OnEventFire]
        public void EnterDisabledState(NodeAddedEvent e, DisabledStateNode node) {
            Animator animator = node.itemButton.Animator;
            ItemButtonAnimationUtils.DisableItemButton(animator);
        }

        [OnEventFire]
        public void DeactivateButtons(NodeAddedEvent e, InventoryItemDisabledStateNode item,
            [JoinBySupply] [Context] ItemButtonNode itemButton) =>
            itemButton.itemButtonESM.Esm.ChangeState<ItemButtonStates.ItemButtonDisabledStateNode>();

        public class DisabledStateNode : Node {
            public ItemButtonComponent itemButton;
            public ItemButtonDisabledStateComponent itemButtonDisabledState;
        }

        public class InventoryItemDisabledStateNode : Node {
            public InventoryItemDisabledStateComponent inventoryItemDisabledState;

            public SupplyGroupComponent supplyGroup;
        }

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ItemButtonESMComponent itemButtonESM;

            public SupplyGroupComponent supplyGroup;
        }
    }
}