using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModulesScreenSystem : ECSSystem {
        [OnEventComplete]
        public void SlotSelected(NodeAddedEvent e, SelectedSlotNode selectedSlot, [JoinByModule] Optional<UserModuleNode> mountedUserModule,
            [JoinAll] SingleNode<ModulesScreenUIComponent> screen) {
            screen.component.FilterCards(!mountedUserModule.IsPresent() ? null : mountedUserModule.Get().Entity, selectedSlot.slotUserItemInfo.ModuleBehaviourType);
            screen.component.ModuleActive = selectedSlot.slotUserItemInfo.ModuleBehaviourType == ModuleBehaviourType.ACTIVE;
        }

        [OnEventFire]
        public void MarketModuleSelected(NodeAddedEvent e, SelectedMarketModuleNode selectedMarketModule,
            [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots,
            [JoinAll] SelectedSlotNode selectedSlot) {
            ModuleSelected(selectedMarketModule, moduleEffectUpgradableProperties, screen, slots, selectedSlot);
        }

        [OnEventFire]
        public void UserModuleSelected(NodeAddedEvent e, SelectedUserModuleNode selectedUserModuleNode,
            [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots,
            [JoinAll] SelectedSlotNode selectedSlot) {
            ModuleSelected(selectedUserModuleNode, moduleEffectUpgradableProperties, screen, slots, selectedSlot);
        }

        [OnEventFire]
        public void UserModuleUpgraded(ModuleUpgradedEvent e, SelectedUserModuleNode selectedUserModuleNode,
            [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots,
            [JoinAll] SelectedSlotNode selectedSlot) {
            ModuleSelected(selectedUserModuleNode, moduleEffectUpgradableProperties, screen, slots, selectedSlot);
        }

        [OnEventFire]
        public void ModuleMounted(NodeAddedEvent e, SelectedSlotWithModuleNode selectedSlotWithModule, [JoinAll] SelectedUserModuleNode selectedUserModuleNode,
            [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots) {
            ModuleSelected(selectedUserModuleNode, moduleEffectUpgradableProperties, screen, slots, selectedSlotWithModule);
        }

        [OnEventFire]
        public void ModuleUnmounted(NodeRemoveEvent e, SelectedSlotWithModuleNode selectedSlotWithModule, [JoinAll] SelectedUserModuleNode selectedUserModuleNode,
            [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots) {
            ModuleSelected(selectedUserModuleNode, moduleEffectUpgradableProperties, screen, slots, selectedSlotWithModule);
        }

        public void ModuleSelected(ModuleNode moduleNode, ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, ScreenNode screen, ICollection<SlotNode> slots,
            SelectedSlotNode selectedSlot) {
            if (selectedSlot.slotUserItemInfo.ModuleBehaviourType != moduleNode.moduleBehaviourType.Type) {
                foreach (SlotNode slot in slots) {
                    if (!slot.slotUI.Locked && slot.slotUserItemInfo.ModuleBehaviourType == moduleNode.moduleBehaviourType.Type &&
                        slot.slotUI.TankPart == moduleNode.moduleTankPart.TankPart && Select<UserModuleNode>(slot.Entity, typeof(ModuleGroupComponent)).Count <= 0) {
                        slot.slotUI.GetComponent<ToggleListItemComponent>().Toggle.isOn = true;
                        break;
                    }
                }
            }

            screen.modulesScreenUi.ModuleName = moduleNode.descriptionItem.Name;
            screen.modulesScreenUi.ModuleDesc = moduleNode.descriptionItem.Description;
            screen.modulesScreenUi.ModuleFlavorText = moduleNode.descriptionItem.Flavor;
            screen.modulesScreenUi.ModuleIconUID = moduleNode.itemIcon.SpriteUid;
            int num = (int)(!moduleNode.Entity.HasComponent<ModuleUpgradeLevelComponent>() ? -1 : moduleNode.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level);
            int count = moduleNode.moduleCardsComposition.UpgradePrices.Count;

            if (num != -1 && num != count) {
                screen.modulesScreenUi.CurrentUpgradeLevel = num + 1;
                screen.modulesScreenUi.NextUpgradeLevel = num + 2;
            } else {
                ModulesScreenUIComponent modulesScreenUi = screen.modulesScreenUi;
                int num2 = -1;
                screen.modulesScreenUi.NextUpgradeLevel = num2;
                modulesScreenUi.CurrentUpgradeLevel = num2;
            }

            screen.modulesScreenUi.ModulesProperties.Clear();
            GetItemStatEvent getItemStatEvent = new();
            ScheduleEvent(getItemStatEvent, moduleNode);
            ModuleUpgradeCharacteristic moduleUpgradeCharacteristic = getItemStatEvent.moduleUpgradeCharacteristic;

            screen.modulesScreenUi.ModulesProperties.AddProperty(moduleUpgradeCharacteristic.Name, moduleUpgradeCharacteristic.Unit, moduleUpgradeCharacteristic.CurrentStr,
                moduleUpgradeCharacteristic.NextStr, moduleUpgradeCharacteristic.Min, moduleUpgradeCharacteristic.Max, moduleUpgradeCharacteristic.Current,
                moduleUpgradeCharacteristic.Next, string.Empty);

            for (int i = 0; i < moduleEffectUpgradableProperties.moduleVisualProperties.Properties.Count; i++) {
                ModuleVisualProperty moduleVisualProperty = moduleEffectUpgradableProperties.moduleVisualProperties.Properties[i];

                if (moduleVisualProperty.Upgradable && num != count && num != -1) {
                    float minValue = 0f;
                    float maxValue = moduleVisualProperty.CalculateModuleEffectPropertyValue(count, count);
                    float currentValue = num == -1 ? 0f : moduleVisualProperty.CalculateModuleEffectPropertyValue(num, count);
                    float nextValue = moduleVisualProperty.CalculateModuleEffectPropertyValue(num + 1, count);

                    screen.modulesScreenUi.ModulesProperties.AddProperty(moduleVisualProperty.Name, moduleVisualProperty.Unit, minValue, maxValue, currentValue, nextValue,
                        moduleVisualProperty.Format);
                } else if (num == -1) {
                    float currentValue2 = moduleVisualProperty.CalculateModuleEffectPropertyValue(0, count);
                    screen.modulesScreenUi.ModulesProperties.AddProperty(moduleVisualProperty.Name, moduleVisualProperty.Unit, currentValue2, moduleVisualProperty.Format);
                } else {
                    float currentValue3 = moduleVisualProperty.CalculateModuleEffectPropertyValue(count, count);
                    screen.modulesScreenUi.ModulesProperties.AddProperty(moduleVisualProperty.Name, moduleVisualProperty.Unit, currentValue3, moduleVisualProperty.Format);
                }
            }
        }

        [OnEventFire]
        public void MountModule(ButtonClickEvent e, SingleNode<MountModuleButtonComponent> mountButton, [JoinAll] SelectedSlotNode selectedSlotNode,
            [JoinAll] SelectedUserModuleNode selectedModule) {
            if (mountButton.component.mount) {
                NewEvent<ModuleMountEvent>().AttachAll(selectedModule, selectedSlotNode).Schedule();
            } else {
                NewEvent<UnmountModuleFromSlotEvent>().AttachAll(selectedModule, selectedSlotNode).Schedule();
            }
        }

        [OnEventFire]
        public void UpgradeModule(ButtonClickEvent e, SingleNode<UpgradeModuleButtonComponent> mountButton, [JoinAll] SelectedUserModuleWithUINode module,
            [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ScreenNode screen, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens,
            [JoinAll] SelfUserMoneyNode selfUserMoneyNode) {
            if (!module.moduleCardItemUI.UpgradeAvailable) {
                return;
            }

            int level = module.moduleCardItemUI.Level;
            List<ModulePrice> upgradePrices = module.moduleCardsComposition.UpgradePrices;
            int num = 0;

            if (level > -1 && level - 1 < upgradePrices.Count) {
                num = upgradePrices[level - 1].Crystals;
            }

            if (num < selfUserMoneyNode.userMoney.Money) {
                ShowUpgradeConfirmDialog(module, dialogs, screens);
                return;
            }

            MainScreenComponent.Instance.ShowShopIfNotVisible();

            if (ShopTabManager.Instance == null) {
                ShopTabManager.shopTabIndex = 4;
            } else {
                ShopTabManager.Instance.Show(4);
            }
        }

        [OnEventFire]
        public void UpgradeXCryModule(ButtonClickEvent e, SingleNode<UpgradeXCryModuleButtonComponent> mountButton, [JoinAll] SelectedUserModuleWithUINode module,
            [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ScreenNode screen, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens,
            [JoinAll] ICollection<ChestItem> chests, [JoinAll] SelfUserMoneyNode selfUserMoneyNode) {
            if (module.moduleCardItemUI.UpgradeAvailable) {
                int level = module.moduleCardItemUI.Level;
                List<ModulePrice> upgradePrices = module.moduleCardsComposition.UpgradePrices;
                int num = 0;

                if (level > -1 && level - 1 < upgradePrices.Count) {
                    num = upgradePrices[level - 1].XCrystals;
                }

                if (num < selfUserMoneyNode.userXCrystals.Money) {
                    ShowUpgradeXCryConfirmDialog(module, dialogs, screens);
                } else {
                    ScheduleEvent(new GoToXCryShopScreen(), selfUserMoneyNode);
                }
            } else {
                GoToChest(module.moduleTier.TierNumber, chests);
            }
        }

        void ShowAssemblyConfirmDialog(ModuleNode module, SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ShowDialog(module, dialogs, screens);
        }

        void ShowUpgradeConfirmDialog(ModuleNode module, SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ShowDialog(module, dialogs, screens);
        }

        void ShowUpgradeXCryConfirmDialog(ModuleNode module, SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ShowDialog(module, dialogs, screens, true);
        }

        void ShowDialog(ModuleNode module, SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens, bool useXCry = false) {
            bool flag = module is MarketModuleNode;
            int num = 0;

            if (!flag && module.Entity.HasComponent<ModuleUpgradeLevelComponent>()) {
                num = (int)module.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level;

                if (num >= module.moduleCardsComposition.UpgradePrices.Count) {
                    return;
                }
            }

            bool dontenoughtcard = false;

            if (flag) {
                dontenoughtcard = !module.Entity.GetComponent<ModuleCardItemUIComponent>().ActivateAvailable;
            }

            double price = flag ? module.moduleCardsComposition.CraftPrice.Crystals :
                           !useXCry ? module.moduleCardsComposition.UpgradePrices[num].Crystals : module.moduleCardsComposition.UpgradePrices[num].XCrystals;

            CraftModuleConfirmWindowComponent craftModuleConfirmWindowComponent = dialogs.component.Get<CraftModuleConfirmWindowComponent>();

            craftModuleConfirmWindowComponent.Setup(module.descriptionItem.Name, module.descriptionItem.Description, module.itemIcon.SpriteUid, price, flag, !useXCry ? "8" : "9",
                dontenoughtcard);

            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            craftModuleConfirmWindowComponent.Show(animators);
            Entity entity = craftModuleConfirmWindowComponent.GetComponent<EntityBehaviour>().Entity;

            if (flag) {
                if (module.Entity.GetComponent<ModuleCardItemUIComponent>().ActivateAvailable) {
                    entity.AddComponent<ModuleAssembleConfirmWindowComponent>();
                } else {
                    entity.AddComponent(new ModuleAssembleNotEnouthCardWindowComponent(module.moduleTier.TierNumber));
                }
            } else if (useXCry) {
                entity.AddComponent<ModuleUpgradeXCryConfirmWindowComponent>();
            } else {
                entity.AddComponent<ModuleUpgradeConfirmWindowComponent>();
            }

            module.Entity.GetComponent<MarketItemGroupComponent>().Attach(entity);
        }

        [OnEventFire]
        public void SendAssemble(DialogConfirmEvent e, SingleNode<ModuleAssembleConfirmWindowComponent> window, [JoinByMarketItem] MarketModuleNode module,
            [JoinAll] SingleNode<SelfUserComponent> user) {
            NewEvent<ModuleAssembleEvent>().Attach(module).Attach(user).Schedule();
        }

        [OnEventFire]
        public void SendAssemble(DialogConfirmEvent e, SingleNode<ModuleAssembleNotEnouthCardWindowComponent> window, [JoinAll] ICollection<ChestItem> chests) {
            GoToChest(window.component.Tier, chests);
        }

        void GoToChest(int tier, ICollection<ChestItem> chests) {
            IEnumerable<ChestItem> source = chests.Where(chest =>
                chest.Entity.HasComponent<UserItemComponent>() && chest.Entity.GetComponent<UserItemCounterComponent>().Count > 0 && chest.targetTier.MaxExistTier >= tier ? true : false);

            ChestItem chestItem = null;

            if (source.Any()) {
                int minTier = source.Min(x => x.targetTier.MaxExistTier);

                chestItem = (from x in source
                             where x.targetTier.MaxExistTier == minTier
                             orderby Select<MarketChestItem>(x.Entity, typeof(MarketItemGroupComponent)).First().xPriceItem.Price
                             select x).FirstOrDefault();
            }

            if (chestItem == null) {
                chestItem = (from chest in chests
                             where chest.Entity.HasComponent<PackPriceComponent>() && chest.targetTier.MaxExistTier >= tier
                             select chest
                             into x
                             orderby x.targetTier.MaxExistTier
                             select x).FirstOrDefault();
            }

            ScheduleEvent(new ShowGarageCategoryEvent {
                Category = GarageCategory.BLUEPRINTS,
                SelectedItem = chestItem.Entity
            }, chestItem);
        }

        [OnEventFire]
        public void SendUpgrade(DialogConfirmEvent e, SingleNode<ModuleUpgradeConfirmWindowComponent> window, [JoinByMarketItem] MarketModuleNode module,
            [JoinByMarketItem] UserModuleNode userModule, [JoinByUser] SingleNode<SelfUserComponent> user) {
            NewEvent<UpgradeModuleWithCrystalsEvent>().Attach(userModule).Schedule();
        }

        [OnEventFire]
        public void SendUpgradeXCry(DialogConfirmEvent e, SingleNode<ModuleUpgradeXCryConfirmWindowComponent> window, [JoinByMarketItem] MarketModuleNode module,
            [JoinByMarketItem] UserModuleNode userModule, [JoinByUser] SingleNode<SelfUserComponent> user) {
            NewEvent<UpgradeModuleWithXCrystalsEvent>().Attach(userModule).Schedule();
        }

        [OnEventFire]
        public void ShowModuleTooltip(ModuleTooltipShowEvent e, ModuleWithUI moduleNode, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties) {
            ModuleTooltipData moduleTooltipData = GetModuleTooltipData(moduleNode, moduleEffectUpgradableProperties);
            moduleNode.moduleCardItemUI.GetComponent<ModuleTooltipShowComponent>().ShowTooltip(moduleTooltipData);
        }

        [OnEventFire]
        public void ShowSlotTooltip(ModuleTooltipShowEvent e, SingleNode<SlotUIComponent> slot, [JoinByModule] Optional<ModuleNode> module,
            [JoinByMarketItem] Optional<ModuleEffectUpgradablePropertyNode> moduleEffectUpgradableProperties) {
            if (slot.Entity.HasComponent<ModuleGroupComponent>()) {
                if (module.IsPresent() && moduleEffectUpgradableProperties.IsPresent()) {
                    ModuleTooltipData moduleTooltipData = GetModuleTooltipData(module.Get(), moduleEffectUpgradableProperties.Get());
                    slot.component.GetComponent<SlotTooltipShowComponent>().ShowModuleTooltip(moduleTooltipData);
                }
            } else {
                slot.component.GetComponent<SlotTooltipShowComponent>().ShowEmptySlotTooltip();
            }
        }

        ModuleTooltipData GetModuleTooltipData(ModuleNode moduleNode, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties) {
            string name = moduleNode.descriptionItem.Name;
            string description = moduleNode.descriptionItem.Description;
            int upgradeLevel = (int)(!moduleNode.Entity.HasComponent<ModuleUpgradeLevelComponent>() ? -1 : moduleNode.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level);
            int count = moduleNode.moduleCardsComposition.UpgradePrices.Count;
            return new ModuleTooltipData(name, description, upgradeLevel, count, moduleEffectUpgradableProperties.moduleVisualProperties.Properties);
        }

        public class ScreenNode : Node {
            public ModulesScreenUIComponent modulesScreenUi;
        }

        public class SlotNode : Node {
            public SlotUIComponent slotUI;

            public SlotUserItemInfoComponent slotUserItemInfo;
        }

        public class SlotWithModule : Node {
            public ModuleGroupComponent moduleGroup;

            public SlotUIComponent slotUI;
        }

        public class SelectedSlotNode : Node {
            public SlotUserItemInfoComponent slotUserItemInfo;

            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelectedSlotWithModuleNode : SelectedSlotNode {
            public ModuleGroupComponent moduleGroup;
        }

        [Not(typeof(ImmutableModuleItemComponent))]
        public class ModuleNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ItemIconComponent itemIcon;

            public MarketItemGroupComponent marketItemGroup;

            public ModuleBehaviourTypeComponent moduleBehaviourType;

            public ModuleCardsCompositionComponent moduleCardsComposition;
            public ModuleItemComponent moduleItem;

            public ModuleTankPartComponent moduleTankPart;

            public ModuleTierComponent moduleTier;

            public OrderItemComponent orderItem;
        }

        public class ModuleWithUI : ModuleNode {
            public ModuleCardItemUIComponent moduleCardItemUI;
        }

        public class UserModuleNode : ModuleNode {
            public ModuleUpgradeLevelComponent moduleUpgradeLevel;
            public UserItemComponent userItem;
        }

        public class MarketModuleNode : ModuleNode {
            public MarketItemComponent marketItem;
        }

        public class SelectedUserModuleNode : UserModuleNode {
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelectedUserModuleWithUINode : SelectedUserModuleNode {
            public ModuleCardItemUIComponent moduleCardItemUI;
        }

        public class SelectedMarketModuleNode : MarketModuleNode {
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class ModuleEffectUpgradablePropertyNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public ModuleVisualPropertiesComponent moduleVisualProperties;
        }

        public class SelfUserMoneyNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserMoneyComponent userMoney;

            public UserXCrystalsComponent userXCrystals;
        }

        public class ChestItem : Node {
            public GameplayChestItemComponent gameplayChestItem;

            public MarketItemGroupComponent marketItemGroup;

            public TargetTierComponent targetTier;
        }

        public class MarketChestItem : ChestItem {
            public MarketItemComponent marketItem;

            public XPriceItemComponent xPriceItem;
        }
    }
}