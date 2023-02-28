using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientProfile.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.UI.New.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class NewModulesScreenSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void Register(NodeAddedEvent e, SingleNode<ImmutableModuleItemComponent> module) { }

        [OnEventFire]
        public void GoToModulesScreen(GoToModulesScreenEvent e, Node node, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            dialogs.component.Get<NewModulesScreenUIComponent>().Show(e.TankPartModuleType);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screenNode, [JoinAll] SingleNode<SelfUserComponent> user,
            [JoinByUser] ICollection<SlotNode> slotNodes, [JoinAll] SelfUserMoneyNode money) {
            slotNodes = slotNodes.Where(s => s.slotTankPart.TankPart.Equals(TankPartModuleType.TANK) || s.slotTankPart.TankPart.Equals(TankPartModuleType.WEAPON)).ToList();
            screenNode.component.InitSlots(slotNodes);
            screenNode.component.InitMoney(money);
            screenNode.component.UpdateViewInNextFrame();
            DragAndDropController dragAndDropController = screenNode.component.dragAndDropController;
            dragAndDropController.onDrop = (Action<DropDescriptor, DropDescriptor>)Delegate.Combine(dragAndDropController.onDrop, new Action<DropDescriptor, DropDescriptor>(OnDrop));
            screenNode.component.crystalButton.GetComponent<Button>().onClick.AddListener(screenNode.component.Hide);
            screenNode.component.xCrystalButton.GetComponent<Button>().onClick.AddListener(screenNode.component.Hide);
        }

        [OnEventFire]
        public void OnRemoveScreen(NodeRemoveEvent e, SingleNode<NewModulesScreenUIComponent> screenNode) {
            DragAndDropController dragAndDropController = screenNode.component.dragAndDropController;
            dragAndDropController.onDrop = (Action<DropDescriptor, DropDescriptor>)Delegate.Remove(dragAndDropController.onDrop, new Action<DropDescriptor, DropDescriptor>(OnDrop));
            screenNode.component.crystalButton.GetComponent<Button>().onClick.RemoveListener(screenNode.component.Hide);
            screenNode.component.xCrystalButton.GetComponent<Button>().onClick.RemoveListener(screenNode.component.Hide);
        }

        void OnDrop(DropDescriptor dropDescriptor, DropDescriptor backDescriptor) {
            if (IsTankSlot(dropDescriptor.sourceCell)) {
                Entity entity = dropDescriptor.sourceCell.GetComponent<TankSlotView>().SlotNode.Entity;
                ModuleItem moduleItem = dropDescriptor.item.GetComponent<SlotItemView>().ModuleItem;
                EngineService.Engine.NewEvent<UnmountModuleFromSlotEvent>().AttachAll(moduleItem.UserItem, entity).Schedule();
            }

            if (backDescriptor.item != null && IsTankSlot(backDescriptor.sourceCell)) {
                Entity entity = backDescriptor.sourceCell.GetComponent<TankSlotView>().SlotNode.Entity;
                ModuleItem moduleItem = backDescriptor.item.GetComponent<SlotItemView>().ModuleItem;
                EngineService.Engine.NewEvent<UnmountModuleFromSlotEvent>().AttachAll(moduleItem.UserItem, entity).Schedule();
            }

            if (IsTankSlot(dropDescriptor.destinationCell)) {
                Entity entity = dropDescriptor.destinationCell.GetComponent<TankSlotView>().SlotNode.Entity;
                ModuleItem moduleItem = dropDescriptor.item.GetComponent<SlotItemView>().ModuleItem;

                if (IsOwnModule(moduleItem.UserItem)) {
                    EngineService.Engine.NewEvent<ModuleMountEvent>().AttachAll(moduleItem.UserItem, entity).Schedule();
                }
            }

            if (backDescriptor.item != null && IsTankSlot(backDescriptor.destinationCell)) {
                Entity entity = backDescriptor.destinationCell.GetComponent<TankSlotView>().SlotNode.Entity;
                ModuleItem moduleItem = backDescriptor.item.GetComponent<SlotItemView>().ModuleItem;

                if (IsOwnModule(moduleItem.UserItem)) {
                    EngineService.Engine.NewEvent<ModuleMountEvent>().AttachAll(moduleItem.UserItem, entity).Schedule();
                }
            }
        }

        bool IsOwnModule(Entity item) => Select<SingleNode<SelfUserComponent>>(item, typeof(UserGroupComponent)).Count > 0;

        bool IsTankSlot(DragAndDropCell slot) => slot.GetComponent<TankSlotView>() != null;

        [OnEventFire]
        public void ResearchModule(ButtonClickEvent e, SingleNode<ResearchModuleButtonComponent> mountButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen,
            [JoinAll] SingleNode<SelfUserComponent> user) {
            Entity marketItem = screen.component.SelectedModule.MarketItem;
            NewEvent<ModuleAssembleEvent>().Attach(marketItem).Attach(user).Schedule();
        }

        [OnEventFire]
        public void UpgradeModuleWithCRY(ButtonClickEvent e, SingleNode<UpgradeModuleButtonComponent> upgradeButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen,
            [JoinAll] SelfUserMoneyNode selfUserMoneyNode) {
            Entity userItem = screen.component.SelectedModule.UserItem;

            if (screen.component.SelectedModule.UpgradePriceCRY <= selfUserMoneyNode.userMoney.Money) {
                NewEvent<UpgradeModuleWithCrystalsEvent>().Attach(userItem).Schedule();
                return;
            }

            screen.component.Hide();
            MainScreenComponent.Instance.ShowShopIfNotVisible();

            if (ShopTabManager.Instance == null) {
                ShopTabManager.shopTabIndex = 4;
            } else {
                ShopTabManager.Instance.Show(4);
            }
        }

        [OnEventFire]
        public void UpgradeModuleWithXCRY(ButtonClickEvent e, SingleNode<UpgradeXCryModuleButtonComponent> upgradeButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen,
            [JoinAll] SelfUserMoneyNode selfUserMoneyNode, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> windows) {
            if (screen.component.SelectedModule.UpgradePriceXCRY <= selfUserMoneyNode.userXCrystals.Money) {
                ShowUpgradeXCryConfirmDialog(dialogs, windows, screen);
                return;
            }

            screen.component.Hide();
            ScheduleEvent(new GoToXCryShopScreen(), selfUserMoneyNode);
        }

        void ShowUpgradeXCryConfirmDialog(SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> window, SingleNode<NewModulesScreenUIComponent> screen) {
            ShowDialog(dialogs, window, screen);
        }

        void ShowDialog(SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens, SingleNode<NewModulesScreenUIComponent> screen, bool useXCry = true) {
            CraftModuleConfirmWindowComponent craftModuleConfirmWindowComponent = dialogs.component.Get<CraftModuleConfirmWindowComponent>();
            ModuleItem selectedModule = screen.component.SelectedModule;

            craftModuleConfirmWindowComponent.Setup(selectedModule.Name, selectedModule.Description(), selectedModule.CardSpriteUid, selectedModule.UpgradePriceXCRY, false,
                !useXCry ? "8" : "9");

            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            craftModuleConfirmWindowComponent.Show(animators);
            Entity entity = craftModuleConfirmWindowComponent.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent<ModuleUpgradeXCryConfirmWindowComponent>();
        }

        [OnEventFire]
        public void SendUpgradeXCry(DialogConfirmEvent e, SingleNode<ModuleUpgradeXCryConfirmWindowComponent> upgradeButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen,
            [JoinAll] SelfUserMoneyNode selfUserMoneyNode, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> windows) {
            Entity userItem = screen.component.SelectedModule.UserItem;
            NewEvent<UpgradeModuleWithXCrystalsEvent>().Attach(userItem).Schedule();
        }

        [OnEventFire]
        public void BuyBlueprints(ButtonClickEvent e, SingleNode<BuyBlueprintsButtonComponent> buyButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen,
            [JoinAll] ICollection<ChestItem> chests) {
            screen.component.Hide();
            GoToChest(screen.component.SelectedModule.TierNumber, screen.component.SelectedModule.MarketCardItem.Id, chests);
        }

        void GoToChest(int tier, long id, ICollection<ChestItem> chests) {
            IEnumerable<ChestItem> source = chests.Where(chest =>
                chest.Entity.HasComponent<UserItemComponent>() && chest.Entity.GetComponent<UserItemCounterComponent>().Count > 0 && chest.targetTier.MaxExistTier >= tier &&
                (chest.targetTier.ContainsAllTierItem || chest.targetTier.ItemList.Contains(id)) ? true : false);

            ChestItem chestItem = null;

            if (source.Any()) {
                chestItem = (from x in source
                             orderby x.orderItem.Index descending, x.targetTier.TargetTier descending
                             select x).FirstOrDefault();
            }

            if (chestItem == null) {
                chestItem = (from chest in chests
                             where chest.targetTier.TargetTier == tier && (chest.targetTier.ContainsAllTierItem || chest.targetTier.ItemList.Contains(id)) &&
                                   chest.Entity.HasComponent<PackPriceComponent>()
                             select chest
                             into x
                             orderby x.targetTier.TargetTier descending
                             select x).FirstOrDefault();
            }

            ScheduleEvent(new ShowGarageCategoryEvent {
                Category = GarageCategory.BLUEPRINTS,
                SelectedItem = chestItem.Entity
            }, chestItem);
        }

        [OnEventFire]
        public void InitTurret(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screen, SelfUserNode self, [JoinByUser] [Context] MountedTurretNode turret,
            [JoinByMarketItem] [Context] ParentGarageMarketItemNode marketItem, [JoinByParentGroup] [Context] MountedSkin mountedSkin, [JoinByUser] [Context] SelfUserNode self2) {
            screen.component.UpdateViewInNextFrame();
            screen.component.turretCollectionView.preview.SpriteUid = mountedSkin.imageItem.SpriteUid;
            screen.component.turretCollectionView.partName.text = turret.descriptionItem.Name;
            screen.component.turretCollectionView.PartLevel = string.Format("{0}", "(" + screen.component.Level + " " + turret.upgradeLevelItem.Level + ")");
            screen.component.Weapon = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
        }

        [OnEventFire]
        public void InitHull(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screen, SelfUserNode self, [JoinByUser] [Context] MountedHullNode hull,
            [JoinByMarketItem] [Context] ParentGarageMarketItemNode marketItem, [JoinByParentGroup] [Context] MountedSkin mountedSkin, [JoinByUser] [Context] SelfUserNode self2) {
            screen.component.UpdateViewInNextFrame();
            screen.component.hullCollectionView.preview.SpriteUid = mountedSkin.imageItem.SpriteUid;
            screen.component.hullCollectionView.partName.text = hull.descriptionItem.Name;
            screen.component.hullCollectionView.PartLevel = string.Format("{0}", "(" + screen.component.Level + " " + hull.upgradeLevelItem.Level + ")");
            screen.component.Tank = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
        }

        [OnEventFire]
        public void ShowPresetList(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] SingleNode<SelfUserComponent> selfUser,
            [JoinByUser] ICollection<PresetNode> presetsList) {
            List<PresetItem> list = new();

            foreach (PresetNode presets in presetsList) {
                if (presets.presetEquipment.HullId != 0 && presets.presetEquipment.WeaponId != 0) {
                    Entity entityById = GetEntityById(presets.presetEquipment.HullId);
                    Entity entityById2 = GetEntityById(presets.presetEquipment.WeaponId);
                    string name = entityById.GetComponent<DescriptionItemComponent>().Name;
                    string name2 = entityById2.GetComponent<DescriptionItemComponent>().Name;
                    long key = entityById.GetComponent<MarketItemGroupComponent>().Key;
                    long key2 = entityById2.GetComponent<MarketItemGroupComponent>().Key;
                    list.Add(new PresetItem(presets.presetName.Name, 1, name, name2, key, key2, presets.Entity));
                }
            }

            screen.component.InitPresetsDropDown(list);
        }

        [OnEventFire]
        public void AddUpgradeConfig(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] ModuleUpgradeConfigNode config) {
            screen.component.level2PowerByTier = config.moduleUpgradablePowerConfig.Level2PowerByTier;
        }

        [OnEventFire]
        public void ModuleChanged(ModuleChangedEvent e, Node node, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen) {
            NewEvent<UpdateScreenEvent>().Attach(screen).Schedule();
        }

        [OnEventFire]
        public void UpdateScreenView(UpdateScreenEvent e, SingleNode<NewModulesScreenUIComponent> screen) {
            screen.component.UpdateView();
        }

        public class SelfUserMoneyNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserMoneyComponent userMoney;

            public UserXCrystalsComponent userXCrystals;
        }

        public class SlotNode : Node {
            public SlotTankPartComponent slotTankPart;

            public SlotUserItemInfoComponent slotUserItemInfo;
            public UserItemComponent userItem;
        }

        public class GarageUserItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ExperienceItemComponent experienceItem;
            public MarketItemGroupComponent marketItemGroup;

            public UpgradeLevelItemComponent upgradeLevelItem;

            public UserGroupComponent userGroup;

            public VisualPropertiesComponent visualProperties;
        }

        public class ModuleItemNode : Node {
            public ModuleItemComponent moduleItem;

            public ModuleTankPartComponent moduleTankPart;
            public ModuleTierComponent moduleTier;
        }

        public class MountedTurretNode : GarageUserItemNode {
            public MountedItemComponent mountedItem;

            public WeaponItemComponent weaponItem;
        }

        public class MountedHullNode : GarageUserItemNode {
            public MountedItemComponent mountedItem;

            public TankItemComponent tankItem;
        }

        public class MountedModuleNode : ModuleItemNode {
            public MountedItemComponent mountedItem;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class MountedSkin : Node {
            public DescriptionItemComponent descriptionItem;

            public ImageItemComponent imageItem;

            public MountedItemComponent mountedItem;

            public ParentGroupComponent parentGroup;
            public SkinItemComponent skinItem;
        }

        public class GarageMarketItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public GarageMarketItemRegisteredComponent garageMarketItemRegistered;

            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;

            public VisualPropertiesComponent visualProperties;
        }

        public class ParentGarageMarketItemNode : GarageMarketItemNode {
            public ParentGroupComponent parentGroup;
        }

        public class PresetNode : Node {
            public PresetEquipmentComponent presetEquipment;
            public PresetItemComponent presetItem;

            public PresetNameComponent presetName;

            public UserItemComponent userItem;
        }

        public class ChestItem : Node {
            public GameplayChestItemComponent gameplayChestItem;

            public MarketItemGroupComponent marketItemGroup;

            public OrderItemComponent orderItem;

            public TargetTierComponent targetTier;
        }

        public class MarketChestItem : ChestItem {
            public MarketItemComponent marketItem;

            public XPriceItemComponent xPriceItem;
        }

        public class ModuleUpgradeConfigNode : Node {
            public ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig;
        }
    }
}