using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNavigation.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageListItemContentSystem : ECSSystem {
        [OnEventFire]
        public void SetContentSlotItem(NodeAddedEvent e, GarageListSlotItemNode slot, [JoinAll] SingleNode<SlotsTextsComponent> slotsTexts,
            [JoinAll] SingleNode<ModuleTypesImagesComponent> moduleTypesImages) {
            slot.garageListItemContent.Header.text = slotsTexts.component.Slot2modules[slot.slotUserItemInfo.ModuleBehaviourType];

            GarageListItemContentPreviewComponent garageListItemContentPreviewComponent =
                slot.garageListItemContent.AddPreview(moduleTypesImages.component.moduleType2image[slot.slotUserItemInfo.ModuleBehaviourType]);

            Color color = default;
            ColorUtility.TryParseHtmlString(moduleTypesImages.component.moduleType2color[slot.slotUserItemInfo.ModuleBehaviourType], out color);
            garageListItemContentPreviewComponent.Image.color = color;
        }

        [OnEventFire]
        public void SetItemName(NodeAddedEvent e, GarageListItemNode listItemNode) {
            DescriptionItemComponent descriptionItem = listItemNode.descriptionItem;
            listItemNode.garageListItemContent.Header.text = descriptionItem.Name;
        }

        [OnEventFire]
        public void SetDescriptionItem(ListItemSelectedEvent e, GarageItemNode item, [JoinAll] ItemDescriptionNode descriptionNode) {
            descriptionNode.displayDescriptionItem.SetDescription(item.descriptionItem.Description);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListNotChildItemNode itemNode, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            topPanel.component.NewHeader = itemNode.descriptionItem.Name;
        }

        [OnEventFire]
        public void SetContainerBundleItemImages(NodeAddedEvent e, GarageListContainerBundleItemNode listItemNode) {
            foreach (MarketItemBundle marketItem in listItemNode.bundleContainerContentItem.MarketItems) {
                Entity entity = Flow.Current.EntityRegistry.GetEntity(marketItem.MarketItem);

                if (entity.HasComponent<ImageItemComponent>()) {
                    listItemNode.garageListItemContent.AddPreview(entity.GetComponent<ImageItemComponent>().SpriteUid, marketItem.Amount);
                }
            }

            if (listItemNode.descriptionBundleItem.Names.ContainsKey(listItemNode.bundleContainerContentItem.NameLokalizationKey)) {
                listItemNode.garageListItemContent.Header.text = listItemNode.descriptionBundleItem.Names[listItemNode.bundleContainerContentItem.NameLokalizationKey];
            }
        }

        [OnEventFire]
        public void SetContainerItemImage(NodeAddedEvent e, GarageListContainerSimpleItemNode listItemNode) {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(listItemNode.simpleContainerContentItem.MarketItemId);

            if (entity.HasComponent<ImageItemComponent>()) {
                listItemNode.garageListItemContent.AddPreview(entity.GetComponent<ImageItemComponent>().SpriteUid);
            }

            if (listItemNode.simpleContainerContentItem.NameLokalizationKey != null &&
                listItemNode.descriptionBundleItem.Names.ContainsKey(listItemNode.simpleContainerContentItem.NameLokalizationKey)) {
                listItemNode.garageListItemContent.Header.text = listItemNode.descriptionBundleItem.Names[listItemNode.simpleContainerContentItem.NameLokalizationKey];
            } else {
                listItemNode.garageListItemContent.Header.text = entity.GetComponent<DescriptionItemComponent>().Name;
            }
        }

        [OnEventFire]
        public void UpdateHeader(ListItemSelectedEvent e, GarageListContainerSimpleItemNode listItemNode, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(listItemNode.simpleContainerContentItem.MarketItemId);

            if (entity.HasComponent<PaintItemComponent>()) {
                if (!string.IsNullOrEmpty(listItemNode.simpleContainerContentItem.NameLokalizationKey)) {
                    topPanel.component.CurrentHeader = listItemNode.descriptionBundleItem.Names[listItemNode.simpleContainerContentItem.NameLokalizationKey];
                }
            } else {
                topPanel.component.CurrentHeader = entity.GetComponent<DescriptionItemComponent>().Name;
            }
        }

        [OnEventFire]
        public void UpdateHeader(ListItemSelectedEvent e, GarageListContainerBundleItemNode listItemNode, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            topPanel.component.CurrentHeader = listItemNode.descriptionBundleItem.Names[listItemNode.bundleContainerContentItem.NameLokalizationKey];
        }

        [OnEventFire]
        public void ShowRareText(NodeAddedEvent e, RareGarageListItemNode item) {
            item.garageListItemContent.RareTextVisibility = true;
        }

        [OnEventFire]
        public void SetImageItem(NodeAddedEvent e, GarageListImagedItemNode listItemNode) {
            string spriteUid = listItemNode.imageItem.SpriteUid;

            if (!string.IsNullOrEmpty(spriteUid)) {
                listItemNode.garageListItemContent.AddPreview(spriteUid);
            }
        }

        [OnEventFire]
        public void ResetNonImagedItem(NodeAddedEvent e, GarageListNonImagedMarketItemNode listItemNode, [JoinByParentGroup] DefaultSkinNode skin) {
            string spriteUid = skin.imageItem.SpriteUid;
            listItemNode.garageListItemContent.AddPreview(spriteUid);
        }

        [OnEventFire]
        public void ResetNonImagedItem(NodeAddedEvent e, GarageListNonImagedUserItemNode listItemNode, [JoinByParentGroup] MountedSkinNode skin) {
            string spriteUid = skin.imageItem.SpriteUid;
            listItemNode.garageListItemContent.AddPreview(spriteUid);
        }

        [OnEventFire]
        public void SetUpgradeLevel(NodeAddedEvent e, GarageListItemUpgradeNode item) {
            item.garageListItemContent.UpgradeLevel.text = item.upgradeLevelItem.Level.ToString();
            float num = item.experienceToLevelUpItem.FinalLevelExperience - item.experienceToLevelUpItem.InitLevelExperience;
            float num2 = num - item.experienceToLevelUpItem.RemainingExperience;
            float progressValue = num2 / num;
            item.garageListItemContent.ProgressBar.ProgressValue = progressValue;
            item.garageListItemContent.Arrow.gameObject.SetActive(item.upgradeLevelItem.Level > item.upgradeLevelItem.Level);
        }

        [OnEventFire]
        public void EnablePrice(NodeAddedEvent e, GarageListItemPriceNode item, [JoinAll] ScreenWithVisibleItemsInfoNode screen) {
            if (item.priceItem.IsBuyable) {
                item.garageListItemContent.PriceGameObject.SetActive(true);
            } else if (item.xPriceItem.IsBuyable) {
                item.garageListItemContent.XPriceGameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void SetPrice(NodeAddedEvent e, GarageListItemPriceNode item) {
            ScheduleEvent(new SetPriceEvent {
                Price = item.priceItem.Price,
                OldPrice = item.priceItem.OldPrice,
                XPrice = item.xPriceItem.Price,
                OldXPrice = item.xPriceItem.OldPrice
            }, item);
        }

        [OnEventFire]
        public void EnableUpgrade(NodeAddedEvent e, GarageListUserUpgradeItemNode item, [JoinAll] ScreenWithVisibleItemsInfoNode screen) {
            item.garageListItemContent.UpgradeGameObject.SetActive(true);
        }

        [OnEventFire]
        public void UpdateHeader(ListItemSelectedEvent e, GarageListNotChildItemNode item, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            topPanel.component.CurrentHeader = item.descriptionItem.Name;
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListShellItemNode itemNode, [JoinByParentGroup] WeaponMarketItemNode weaponMarketItem,
            [JoinAll] GarageItemsScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.ShellItemsHeaderText, weaponMarketItem.descriptionItem.Name);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListGraffitiItemNode itemNode, [JoinAll] GarageItemsScreenNode screen,
            [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.GraffitiItemsHeaderText);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, SingleNode<SkinItemComponent> itemNode, [JoinByParentGroup] WeaponMarketItemNode weaponMarketItem,
            [JoinAll] GarageItemsScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.WeaponSkinItemsHeaderText, weaponMarketItem.descriptionItem.Name);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, SingleNode<SkinItemComponent> itemNode, [JoinByParentGroup] HullMarketItemNode hullMarketItemNode,
            [JoinAll] GarageItemsScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.HullSkinItemsHeaderText, hullMarketItemNode.descriptionItem.Name);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListContainerSimpleItemNode listItemNode, [JoinAll] ContainerContentScreenNode screen,
            [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(listItemNode.simpleContainerContentItem.MarketItemId);
            topPanel.component.NewHeader = entity.GetComponent<DescriptionItemComponent>().Name;
        }

        public class ItemDescriptionNode : Node {
            public ActiveScreenComponent activeScreen;

            public DisplayDescriptionItemComponent displayDescriptionItem;
            public ItemsListScreenComponent itemsListScreen;

            public ScreenComponent screen;
        }

        public class GarageListItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public GarageListItemContentComponent garageListItemContent;
        }

        public class GarageListContainerBundleItemNode : Node {
            public BundleContainerContentItemComponent bundleContainerContentItem;

            public DescriptionBundleItemComponent descriptionBundleItem;

            public GarageListItemContentComponent garageListItemContent;
        }

        public class RareGarageListItemNode : Node {
            public GarageListItemContentComponent garageListItemContent;
            public RareContainerContentItemComponent rareContainerContentItem;
        }

        public class GarageListContainerSimpleItemNode : Node {
            public DescriptionBundleItemComponent descriptionBundleItem;

            public GarageListItemContentComponent garageListItemContent;
            public SimpleContainerContentItemComponent simpleContainerContentItem;
        }

        [Not(typeof(ImageItemComponent))]
        public class GarageListNonImagedItemNode : GarageListItemNode { }

        public class GarageListNonImagedUserItemNode : GarageListNonImagedItemNode {
            public UserItemComponent userItem;
        }

        public class GarageListNonImagedMarketItemNode : GarageListNonImagedItemNode {
            public MarketItemComponent marketItem;
        }

        public class GarageListImagedItemNode : GarageListItemNode {
            public ImageItemComponent imageItem;
        }

        public class GarageListItemUpgradeNode : Node {
            public ExperienceToLevelUpItemComponent experienceToLevelUpItem;
            public GarageListItemContentComponent garageListItemContent;

            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class GarageListItemPriceNode : Node {
            public GarageListItemContentComponent garageListItemContent;
            public MarketItemComponent marketItem;

            public PriceItemComponent priceItem;

            public PriceLabelComponent priceLabel;

            public XPriceItemComponent xPriceItem;

            public XPriceLabelComponent xPriceLabel;
        }

        public class GarageListUserUpgradeItemNode : Node {
            public GarageListItemContentComponent garageListItemContent;
            public UserItemComponent userItem;
        }

        public class SkinNode : Node {
            public ImageItemComponent imageItem;
            public SkinItemComponent skinItem;
        }

        public class MountedSkinNode : SkinNode {
            public MountedItemComponent mountedItem;
        }

        public class DefaultSkinNode : SkinNode {
            public DefaultSkinItemComponent defaultSkinItem;
        }

        [Not(typeof(GraffitiItemComponent))]
        [Not(typeof(SkinItemComponent))]
        [Not(typeof(ShellItemComponent))]
        public class GarageListNotChildItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public GarageListItemComponent garageListItem;
        }

        public class GarageListShellItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public GarageItemComponent garageItem;

            public GarageListItemComponent garageListItem;
            public ShellItemComponent shellItem;
        }

        public class GarageItemsScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public GarageItemsScreenTextComponent garageItemsScreenText;
        }

        public class ContainerContentScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public ContainerContentScreenComponent containerContentScreen;
        }

        public class WeaponMarketItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MarketItemComponent marketItem;

            public ParentGroupComponent parentGroup;

            public WeaponItemComponent weaponItem;
        }

        public class HullMarketItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MarketItemComponent marketItem;

            public ParentGroupComponent parentGroup;

            public TankItemComponent tankItem;
        }

        public class GarageListGraffitiItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public GarageItemComponent garageItem;

            public GarageListItemComponent garageListItem;
            public GraffitiItemComponent graffitiItem;
        }

        public class GarageItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;
        }

        [Not(typeof(ContainerContentScreenComponent))]
        [Not(typeof(ContainersScreenComponent))]
        public class ScreenWithVisibleItemsInfoNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class GarageListSlotItemNode : Node {
            public GarageListItemComponent garageListItem;

            public GarageListItemContentComponent garageListItemContent;

            public SlotTankPartComponent slotTankPart;
            public SlotUserItemInfoComponent slotUserItemInfo;
        }
    }
}