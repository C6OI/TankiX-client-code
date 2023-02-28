using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModulesListSystem : ECSSystem {
        [OnEventFire]
        public void ModulesMarketItemsInit(NodeAddedEvent e, SingleNode<ModulesScreenUIComponent> screen, [Combine] MarketModuleNode module) {
            screen.component.AddCard(module.Entity);
        }

        [OnEventFire]
        public void ModuleInited(NodeAddedEvent e, ModuleWithUINode moduleWithUI, [JoinByParentGroup] Optional<ModuleCardNode> moduleCard) {
            moduleWithUI.moduleCardItemUi.Type = moduleWithUI.moduleBehaviourType.Type;
            moduleWithUI.moduleCardItemUi.TankPart = moduleWithUI.moduleTankPart.TankPart;
            moduleWithUI.moduleCardItemUi.Icon = moduleWithUI.itemBigIcon.SpriteUid;
            moduleWithUI.moduleCardItemUi.Level = -1;
            moduleWithUI.moduleCardItemUi.ModuleName = moduleWithUI.descriptionItem.Name;
            moduleWithUI.moduleCardItemUi.MaxCardsCount = moduleWithUI.moduleCardsComposition.CraftPrice.Cards;
            moduleWithUI.moduleCardItemUi.CardsCount = (int)(moduleCard.IsPresent() ? moduleCard.Get().userItemCounter.Count : 0);
            moduleWithUI.moduleCardItemUi.Active = moduleWithUI.moduleBehaviourType.Type == ModuleBehaviourType.ACTIVE;
        }

        [OnEventFire]
        public void ModuleUserInited(NodeAddedEvent e, [Combine] UserModuleNode userModule, [JoinByMarketItem] [Context] ModuleWithUINode moduleWithUI,
            [JoinByParentGroup] Optional<ModuleCardNode> moduleCards, [JoinAll] Optional<SelectedSlotWithModuleNode> selectedSlot, [JoinAll] SelfUserNode selfUser) {
            if (userModule.userGroup.Key == selfUser.userGroup.Key) {
                moduleWithUI.Entity.RemoveComponent<ModuleCardItemUIComponent>();
                moduleWithUI.Entity.RemoveComponent<ToggleListItemComponent>();

                if (moduleWithUI.Entity.HasComponent<ToggleListSelectedItemComponent>()) {
                    moduleWithUI.Entity.RemoveComponent<ToggleListSelectedItemComponent>();
                }

                if (userModule.Entity.HasComponent<ModuleCardItemUIComponent>()) {
                    userModule.Entity.RemoveComponent<ModuleCardItemUIComponent>();
                }

                if (userModule.Entity.HasComponent<ToggleListItemComponent>()) {
                    userModule.Entity.RemoveComponent<ToggleListItemComponent>();
                }

                userModule.Entity.AddComponent(moduleWithUI.moduleCardItemUi);
                userModule.Entity.AddComponent(moduleWithUI.moduleCardItemUi.GetComponent<ToggleListItemComponent>());
                SetModuleLevel(moduleWithUI.moduleCardItemUi, userModule.moduleUpgradeLevel, moduleWithUI.moduleCardsComposition, moduleCards);
                SelectMountedModuleCard(moduleWithUI.moduleCardItemUi, userModule.moduleGroup, selectedSlot);
            }
        }

        [OnEventFire]
        public void ModuleWasUpgrade(ModuleUpgradedEvent e, UserModuleWithUINode userModuleWithUI, [JoinByParentGroup] Optional<ModuleCardNode> moduleCards,
            [JoinAll] Optional<SelectedSlotWithModuleNode> selectedSlot) {
            SetModuleLevel(userModuleWithUI.moduleCardItemUi, userModuleWithUI.moduleUpgradeLevel, userModuleWithUI.moduleCardsComposition, moduleCards);
            SelectMountedModuleCard(userModuleWithUI.moduleCardItemUi, userModuleWithUI.moduleGroup, selectedSlot);
        }

        void SetModuleLevel(ModuleCardItemUIComponent moduleCardItemUi, ModuleUpgradeLevelComponent moduleUpgradeLevel, ModuleCardsCompositionComponent moduleCardsComposition,
            Optional<ModuleCardNode> moduleCards) {
            int num = (int)moduleUpgradeLevel.Level;
            moduleCardItemUi.Level = num + 1;
            List<ModulePrice> upgradePrices = moduleCardsComposition.UpgradePrices;
            moduleCardItemUi.MaxCardsCount = num < upgradePrices.Count ? upgradePrices[num].Cards : 0;
            moduleCardItemUi.CardsCount = (int)(moduleCards.IsPresent() ? moduleCards.Get().userItemCounter.Count : 0);
        }

        void SelectMountedModuleCard(ModuleCardItemUIComponent moduleCardItemUi, ModuleGroupComponent userModuleGroup, Optional<SelectedSlotWithModuleNode> selectedSlot) {
            ToggleListItemComponent component = moduleCardItemUi.GetComponent<ToggleListItemComponent>();

            if (selectedSlot.IsPresent() && selectedSlot.Get().moduleGroup.Key == userModuleGroup.Key) {
                component.Toggle.isOn = true;
            } else if (component.Toggle.isOn) {
                component.OnValueChangedListener();
            }
        }

        [OnEventFire]
        public void ClearModules(NodeRemoveEvent e, SingleNode<ModulesScreenUIComponent> screen) {
            screen.component.Clear();
        }

        public class SelectedSlot : Node {
            public SlotUserItemInfoComponent slotUserItemInfo;

            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        [Not(typeof(ImmutableModuleItemComponent))]
        public class ModuleNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ItemBigIconComponent itemBigIcon;

            public ItemIconComponent itemIcon;

            public MarketItemGroupComponent marketItemGroup;

            public ModuleBehaviourTypeComponent moduleBehaviourType;

            public ModuleCardsCompositionComponent moduleCardsComposition;
            public ModuleItemComponent moduleItem;

            public ModuleTankPartComponent moduleTankPart;

            public ModuleTierComponent moduleTier;

            public OrderItemComponent orderItem;
        }

        public class UserModuleNode : ModuleNode {
            public ModuleGroupComponent moduleGroup;

            public ModuleUpgradeLevelComponent moduleUpgradeLevel;

            public UserGroupComponent userGroup;
            public UserItemComponent userItem;
        }

        public class UserModuleWithUINode : UserModuleNode {
            public ModuleCardItemUIComponent moduleCardItemUi;

            public ToggleListItemComponent toggleListItem;
        }

        public class MarketModuleNode : ModuleNode {
            public MarketItemComponent marketItem;
        }

        public class ModuleWithUINode : MarketModuleNode {
            public ModuleCardItemUIComponent moduleCardItemUi;

            public ToggleListItemComponent toggleListItem;
        }

        public class SelectedSlotWithModuleNode : Node {
            public ModuleGroupComponent moduleGroup;
            public SlotUIComponent slotUI;

            public SlotUserItemInfoComponent slotUserItemInfo;

            public ToggleListSelectedItemComponent toggleListSelectedItem;
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
    }
}