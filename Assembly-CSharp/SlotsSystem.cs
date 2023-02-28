using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

public class SlotsSystem : ECSSystem {
    [OnEventComplete]
    public void ShowUpgradeIcon(NodeAddedEvent e, SlotWithUIAndModuleNode selectedSlot, [JoinByModule] Optional<UserModuleNode> userModule,
        [JoinByParentGroup] Optional<ModuleCardNode> moduleCard, [JoinAll] SelfUserNode selfUser) {
        if (userModule.IsPresent() && userModule.Get().userGroup.Key != selfUser.userGroup.Key) {
            return;
        }

        selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(false);

        if (!moduleCard.IsPresent()) {
            return;
        }

        ModuleCardNode moduleCardNode = moduleCard.Get();
        long count = moduleCardNode.userItemCounter.Count;

        if (!userModule.IsPresent()) {
            return;
        }

        long num = userModule.Get().moduleUpgradeLevel.Level + 1;

        if (num <= userModule.Get().moduleCardsComposition.UpgradePrices.Count) {
            long num2 = userModule.Get().moduleCardsComposition.UpgradePrices[(int)(num - 1)].Cards;

            if (num2 <= count) {
                selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(true);
            }
        }
    }

    [OnEventFire]
    public void RemoveUpgradeIcon(NodeRemoveEvent e, SlotWithUIAndModuleNode userModule) {
        userModule.slotUI.UpgradeIcon.gameObject.SetActive(false);
    }

    [OnEventFire]
    public void OnModuleWasUpgraded(ModuleUpgradedEvent e, UserModuleNode userModule, [JoinByModule] SlotWithUIAndModuleNode selectedSlot, UserModuleNode userModule2,
        [JoinByParentGroup] Optional<ModuleCardNode> moduleCard) {
        int num = (int)userModule.moduleUpgradeLevel.Level;
        int count = userModule.moduleCardsComposition.UpgradePrices.Count;

        if (num == count) {
            selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(false);
        } else if (moduleCard.IsPresent()) {
            long num2 = userModule.moduleCardsComposition.UpgradePrices[num].Cards;

            if (num2 > moduleCard.Get().userItemCounter.Count) {
                selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(false);
            }
        } else {
            selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(false);
        }
    }

    [OnEventFire]
    public void SlotsInModulesScreenInit(NodeAddedEvent e, SingleNode<ModulesScreenUIComponent> screen, UserWithoutRentedPreset user,
        [JoinByUser] ICollection<NotReservedSlotNode> slots) {
        IEnumerable<NotReservedSlotNode> enumerable = slots.Where(it => it.Entity.HasComponent<SlotUIComponent>());

        foreach (NotReservedSlotNode item in enumerable) {
            item.Entity.RemoveComponent<SlotUIComponent>();
        }

        foreach (NotReservedSlotNode slot2 in slots) {
            SlotUIComponent slot = screen.component.GetSlot(slot2.slotUserItemInfo.Slot);
            InitSlotUI(slot, slot2);
            InitSlotUIToggle(slot, slot2.Entity);
        }
    }

    [OnEventFire]
    public void SlotsInGarageInited(NodeAddedEvent e, SingleNode<GarageSlotsUIPanelComponent> screen, [Context] UserNode user, [Context] ICollection<NotReservedSlotNode> slots,
        [Context] SelectedPresetNode selectedPreset) {
        IEnumerable<NotReservedSlotNode> enumerable = slots.Where(it => it.Entity.HasComponent<SlotUIComponent>());

        foreach (NotReservedSlotNode item in enumerable) {
            item.Entity.RemoveComponent<SlotUIComponent>();
        }

        IEnumerable<NotReservedSlotNode> enumerable2 = slots.Where(it => it.Entity.GetComponent<UserGroupComponent>().Key == selectedPreset.userGroup.Key);

        foreach (NotReservedSlotNode item2 in enumerable2) {
            SlotUIComponent slot = screen.component.GetSlot(item2.slotUserItemInfo.Slot);
            InitSlotUI(slot, item2);
        }
    }

    [OnEventFire]
    public void SetSettingsSlotIcons(NodeAddedEvent e, SingleNode<SettingsSlotsUIComponent> slotsUI, [JoinAll] ICollection<SlotNode> slots,
        [JoinAll] [Context] SelectedPresetNode selectedPreset) {
        foreach (SlotNode slot2 in slots) {
            SettingsSlotUIComponent slot = slotsUI.component.GetSlot(slot2.slotUserItemInfo.Slot);

            if (slot == null) {
                continue;
            }

            string empty = string.Empty;

            if (slot2.Entity.GetComponent<UserGroupComponent>().Key == selectedPreset.userGroup.Key || slot2.Entity.HasComponent<SlotReservedComponent>()) {
                IList<ModuleNode> list = Select<ModuleNode>(slot2.Entity, typeof(ModuleGroupComponent));
                empty = list.Count <= 0 ? string.Empty : list[0].itemBigIcon.SpriteUid;
                bool moduleActive = false;

                if (list.Count > 0) {
                    IList<ModuleUsesCounterNode> list2 = Select<ModuleUsesCounterNode>(list[0].Entity, typeof(ModuleGroupComponent));
                    moduleActive = list2.Count == 0 || list2.Count > 0 && list2.First().userItemCounter.Count > 0;
                }

                slot.SetIcon(empty, moduleActive);
            }
        }
    }

    void InitSlotUI(SlotUIComponent slotUI, NotReservedSlotNode notReservedSlotNode) {
        if (!(slotUI == null)) {
            slotUI.Locked = false;
            slotUI.Rank = notReservedSlotNode.slotUserItemInfo.UpgradeLevel;
            slotUI.ModuleIcon.color = Color.white;
            slotUI.Slot = notReservedSlotNode.slotUserItemInfo.Slot;
            slotUI.TankPart = notReservedSlotNode.slotTankPart.TankPart;

            if (slotUI.SelectionImage != null) {
                slotUI.SelectionImage.color = Color.white;
            }

            if (notReservedSlotNode.Entity.HasComponent<SlotUIComponent>()) {
                notReservedSlotNode.Entity.RemoveComponent<SlotUIComponent>();
            }

            notReservedSlotNode.Entity.AddComponent(slotUI);
        }
    }

    void InitSlotUIToggle(SlotUIComponent slotUI, Entity slotEntity) {
        ToggleListItemComponent component = slotUI.GetComponent<ToggleListItemComponent>();

        if (slotEntity.HasComponent<ToggleListItemComponent>()) {
            slotEntity.RemoveComponent<ToggleListItemComponent>();
        }

        slotEntity.AddComponent(component);
    }

    [OnEventFire]
    public void SetIcon(NodeAddedEvent e, SlotWithUIAndModuleNode slot, [JoinByModule] ModuleNode module) {
        slot.slotUI.ModuleIconImageSkin.SpriteUid = module.itemBigIcon.SpriteUid;
    }

    [OnEventFire]
    public void ResetIcon(NodeRemoveEvent e, SlotWithUIAndModuleNode slot) {
        slot.slotUI.ModuleIconImageSkin.SpriteUid = null;
    }

    [OnEventFire]
    public void LockSlot(NodeAddedEvent e, [Combine] LockedSlotNode slot, [Context] [JoinByUser] SelectedPresetNode selectedPreset) {
        slot.slotUI.Locked = true;
    }

    [OnEventFire]
    public void UnlockSlot(NodeRemoveEvent e, [Combine] LockedSlotNode slot, [Context] [JoinByUser] SelectedPresetNode selectedPreset) {
        slot.slotUI.Locked = false;
        slot.slotUI.Rank = slot.slotUserItemInfo.UpgradeLevel;
    }

    public class SlotNode : Node {
        public SlotTankPartComponent slotTankPart;
        public SlotUserItemInfoComponent slotUserItemInfo;
    }

    [Not(typeof(SlotReservedComponent))]
    public class NotReservedSlotNode : SlotNode { }

    public class ModuleNode : Node {
        public DescriptionItemComponent descriptionItem;

        public ItemBigIconComponent itemBigIcon;

        public ItemIconComponent itemIcon;

        public ModuleGroupComponent moduleGroup;
        public ModuleItemComponent moduleItem;
    }

    public class SlotWithUIAndModuleNode : Node {
        public ModuleGroupComponent moduleGroup;

        public SlotUIComponent slotUI;
        public SlotUserItemInfoComponent slotUserItemInfo;
    }

    public class LockedSlotNode : NotReservedSlotNode {
        public SlotLockedComponent slotLocked;

        public SlotUIComponent slotUI;
    }

    public class UserModuleNode : ModuleNode {
        public ModuleCardsCompositionComponent moduleCardsComposition;

        public ModuleUpgradeLevelComponent moduleUpgradeLevel;

        public UserGroupComponent userGroup;
        public UserItemComponent userItem;
    }

    public class ModuleCardNode : Node {
        public ModuleCardItemComponent moduleCardItem;

        public UserItemComponent userItem;

        public UserItemCounterComponent userItemCounter;
    }

    public class SelfUserNode : Node {
        public SelfUserComponent selfUser;

        public UserGroupComponent userGroup;
    }

    [Not(typeof(UserUseItemsPrototypeComponent))]
    public class UserWithoutRentedPreset : SelfUserNode { }

    public class UserNode : Node {
        public SelfUserComponent selfUser;
        public UserGroupComponent userGroup;
    }

    public class SelectedPresetNode : Node {
        public SelectedPresetComponent selectedPreset;

        public UserGroupComponent userGroup;
    }

    public class ModuleUsesCounterNode : Node {
        public ModuleUsesCounterComponent moduleUsesCounter;

        public UserGroupComponent userGroup;
        public UserItemComponent userItem;

        public UserItemCounterComponent userItemCounter;
    }
}