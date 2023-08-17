using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonActivatedStateSystem : ECSSystem {
        [OnEventFire]
        public void ActivateFromInventory(NodeAddedEvent e, EffectInventoryNode effect,
            [JoinBySupply] [Context] InventoryItemActivatedNode inventory,
            [Context] [JoinBySupply] ItemButtonActivatedNode button) {
            Animator animator = button.itemButton.Animator;
            ItemButtonAnimationUtils.ActivateItemButtonFromInventory(animator);
        }

        [OnEventFire]
        public void Activate(NodeAddedEvent e, EffectNode effect, [Context] [JoinBySupply] ItemButtonNode button) =>
            button.itemButtonESM.Esm.ChangeState<ItemButtonStates.ItemButtonActivatedStateNode>();

        [OnEventFire]
        public void Activate(NodeAddedEvent evt, EffectNode effect,
            [Context] [JoinBySupply] ItemButtonActivatedNode button) => SetActivatedStateDuration(button, effect);

        static void SetActivatedStateDuration(ItemButtonActivatedNode button, EffectNode effect) {
            float num = effect.durationConfig.Duration / 1000f;
            float unityTime = effect.duration.StartedTime.UnityTime;
            float unityTime2 = Date.Now.UnityTime;
            float activeTime = num - unityTime2 + unityTime;
            Animator animator = button.itemButton.Animator;
            ItemButtonAnimationUtils.SwitchItemButtonToActiveState(animator, activeTime);
        }

        public class ItemButtonActivatedNode : Node {
            public ItemButtonComponent itemButton;

            public ItemButtonActivatedStateComponent itemButtonActivatedState;

            public ItemButtonESMComponent itemButtonESM;

            public SupplyGroupComponent supplyGroup;
        }

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ItemButtonESMComponent itemButtonESM;

            public SupplyGroupComponent supplyGroup;
        }

        public class InventoryItemActivatedNode : Node {
            public InventoryItemActivatedStateComponent inventoryItemActivatedState;
            public SupplyBattleItemComponent supplyBattleItem;

            public SupplyGroupComponent supplyGroup;
        }

        public class EffectNode : Node {
            public DurationComponent duration;

            public DurationConfigComponent durationConfig;

            public EffectComponent effect;
            public IndexComponent index;

            public SupplyGroupComponent supplyGroup;

            public TankGroupComponent tankGroup;
        }

        public class EffectInventoryNode : Node {
            public EffectComponent effect;

            public EffectInventoryComponent effectInventory;

            public SupplyGroupComponent supplyGroup;
        }
    }
}