using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class InventoryHUDSystem : ECSSystem {
        [OnEventFire]
        public void InitSlots(NodeAddedEvent e, SingleNode<InventoryHUDComponent> inventory, HUDNodes.SelfBattleUserAsTankNode selfUser, [JoinByUser] ICollection<ModuleNode> modules) {
            CheckInventoryHUDNecessityEvent checkInventoryHUDNecessityEvent = new();
            ScheduleEvent(checkInventoryHUDNecessityEvent, selfUser);

            if (checkInventoryHUDNecessityEvent.Necessity) {
                ScheduleEvent<InitSlotsEvent>(selfUser);
            }
        }

        [OnEventFire]
        public void CheckInventoryHUDNecessity(CheckInventoryHUDNecessityEvent e, HUDNodes.SelfBattleUserAsTankNode selfUser, [JoinByUser] [Combine] SlotNode slot,
            [JoinByModule] ModuleNode module) {
            IList<ModuleUsesCounterNode> list = Select<ModuleUsesCounterNode>(module.Entity, typeof(ModuleGroupComponent));
            bool flag = list.Count == 0 || list.Count > 0 && list.First().userItemCounter.Count > 0;
            e.Necessity = e.Necessity || flag;
        }

        [OnEventFire]
        public void InitSlots(InitSlotsEvent e, HUDNodes.SelfBattleUserAsTankNode selfUser, [JoinByUser] [Combine] SlotNode slot, [JoinByModule] ModuleNode module,
            [JoinByModule] ICollection<ModuleHUDNode> moduleHuds, [JoinAll] SingleNode<InventoryHUDComponent> inventory) {
            if (moduleHuds.Count <= 0) {
                EntityBehaviour entityBehaviour = inventory.component.CreateModule(slot.slotUserItemInfo.Slot);
                module.moduleGroup.Attach(entityBehaviour.Entity);

                if (module.Entity.HasComponent<GoldBonusModuleItemComponent>()) {
                    inventory.component.CreateGoldBonusesCounter(entityBehaviour);
                }
            }
        }

        [OnEventFire]
        public void InitModule(NodeAddedEvent e, [Context] [Combine] ModuleNode module, [Context] [JoinByModule] SlotNode slot, [Context] [JoinByModule] ModuleHUDNode hud,
            [JoinAll] SingleNode<InventoryHUDComponent> inventory) {
            hud.itemButton.Icon = module.itemIcon.SpriteUid;
            hud.itemButton.KeyBind = inventory.component.GetKeyBindForItem(hud.itemButton);
            hud.itemButton.MaxItemAmmunitionCount = slot.inventoryAmmunition.MaxCount;
            hud.itemButton.ItemAmmunitionCount = slot.inventoryAmmunition.CurrentCount;
        }

        public class ModuleNode : Node {
            public ItemIconComponent itemIcon;
            public ModuleGroupComponent moduleGroup;

            public ModuleItemComponent moduleItem;

            public UserGroupComponent userGroup;
        }

        public class ModuleHUDNode : Node {
            public ItemButtonComponent itemButton;
            public ModuleGroupComponent moduleGroup;
        }

        public class SlotNode : Node {
            public InventoryAmmunitionComponent inventoryAmmunition;

            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;

            public TankGroupComponent tankGroup;

            public UserGroupComponent userGroup;
        }

        public class ModuleUsesCounterNode : Node {
            public ModuleUsesCounterComponent moduleUsesCounter;

            public UserGroupComponent userGroup;
            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }

        public class CheckInventoryHUDNecessityEvent : Event {
            public CheckInventoryHUDNecessityEvent() => Necessity = false;

            public bool Necessity { get; set; }
        }

        public class InitSlotsEvent : Event { }
    }
}