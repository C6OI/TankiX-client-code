using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class InventorySlotActivationSystem : ECSSystem {
        readonly Dictionary<string, Slot> actionToSlotMap = new() {
            {
                InventoryAction.INVENTORY_SLOT1,
                Slot.SLOT1
            }, {
                InventoryAction.INVENTORY_SLOT3,
                Slot.SLOT2
            }, {
                InventoryAction.INVENTORY_SLOT5,
                Slot.SLOT3
            }, {
                InventoryAction.INVENTORY_SLOT2,
                Slot.SLOT4
            }, {
                InventoryAction.INVENTORY_SLOT4,
                Slot.SLOT5
            }, {
                InventoryAction.INVENTORY_SLOT6,
                Slot.SLOT6
            }, {
                InventoryAction.INVENTORY_GOLDBOX,
                Slot.SLOT7
            }
        };

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void TryActivate(UpdateEvent e, HUDNodes.SelfBattleUserAsTankNode self, [JoinByUser] [Combine] HUDNodes.Modules.SlotWithModuleNode slot,
            [JoinByModule] SingleNode<ItemButtonComponent> hud, [JoinByModule] NotAutostartModuleNode module) {
            if (!slot.Entity.HasComponent<InventoryEnabledStateComponent>()) {
                return;
            }

            if (slot.Entity.HasComponent<InventorySlotTemporaryBlockedComponent>()) {
                foreach (KeyValuePair<string, Slot> item in actionToSlotMap) {
                    if (InputManager.GetActionKeyDown(item.Key) && slot.slotUserItemInfo.Slot == item.Value) {
                        hud.component.PressedWhenDisable();
                    }
                }

                return;
            }

            foreach (KeyValuePair<string, Slot> item2 in actionToSlotMap) {
                if (InputManager.GetActionKeyDown(item2.Key) && slot.slotUserItemInfo.Slot == item2.Value) {
                    hud.component.Activate();
                    ScheduleEvent<ActivateModuleEvent>(slot);
                }
            }
        }

        [Not(typeof(PassiveModuleComponent))]
        public class NotAutostartModuleNode : Node {
            public ModuleGroupComponent moduleGroup;
            public ModuleItemComponent moduleItem;

            public UserItemComponent userItem;
        }
    }
}