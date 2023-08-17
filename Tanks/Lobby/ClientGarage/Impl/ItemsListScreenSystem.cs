using System.Collections.Generic;
using System.Linq;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientNavigation.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemsListScreenSystem : ECSSystem {
        [OnEventFire]
        public void AddItems(NodeAddedEvent e, ItemsListNode itemListNode,
            [Context] [JoinByScreen] SingleNode<SimpleHorizontalListComponent> horizontalListNode) =>
            NewEvent<ShowGarageItemsEvent>().AttachAll(itemListNode.itemsListForView.Items).Schedule();

        [OnEventFire]
        [Mandatory]
        public void AddItems(ShowGarageItemsEvent e, ICollection<MarketItem> marketItems, ICollection<UserItem> userItems,
            ICollection<SingleNode<MountedItemComponent>> mountedItems, [JoinAll] ScreenNode screen,
            [JoinByScreen] SingleNode<SimpleHorizontalListComponent> horizontalListNode,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode) {
            SimpleHorizontalListComponent itemsList = horizontalListNode.component;
            List<UserItem> list = userItems.ToList();
            list.Sort((a, b) => a.orderItem.Index.CompareTo(b.orderItem.Index));

            list.ForEach(delegate(UserItem item) {
                itemsList.AddItem(item.Entity);
            });

            List<MarketItem> buyableMarketItems = GetBuyableMarketItems(marketItems, userItems);
            buyableMarketItems.Sort((a, b) => a.orderItem.Index.CompareTo(b.orderItem.Index));

            buyableMarketItems.ForEach(delegate(MarketItem item) {
                itemsList.AddItem(item.Entity);
            });

            Entity entity = selectedItemNode.component.SelectedItem;

            if (entity == null) {
                entity = mountedItems.Count <= 0 ? list.First().Entity : mountedItems.First().Entity;
            }

            ScheduleEvent<SelectGarageItemEvent>(entity);
        }

        static List<MarketItem> GetBuyableMarketItems(ICollection<MarketItem> marketItems, ICollection<UserItem> userItems) {
            Dictionary<long, MarketItem> buyableMarketItems = new();

            marketItems.ForEach(delegate(MarketItem marketItem) {
                buyableMarketItems[marketItem.marketItemGroup.Key] = marketItem;
            });

            userItems.ForEach(delegate(UserItem userItem) {
                buyableMarketItems.Remove(userItem.marketItemGroup.Key);
            });

            return buyableMarketItems.Values.ToList();
        }

        [OnEventFire]
        public void SelectItem(SelectGarageItemEvent e, GarageListItemNode itemNode, [JoinAll] ScreenNode screen,
            [JoinByScreen] SingleNode<SimpleHorizontalListComponent> listNode) {
            Entity entity = itemNode.Entity;
            SimpleHorizontalListComponent component = listNode.component;
            component.Select(entity);
            component.MoveToItem(entity);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListNotChildItemNode itemNode,
            [JoinAll] SingleNode<TopPanelComponent> topPanel) =>
            topPanel.component.NewHeader = itemNode.descriptionItem.name;

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListShellItemNode itemNode,
            [JoinByParentGroup] WeaponMarketItemNode weaponMarketItem, [JoinAll] GarageItemsScreenNode screen,
            [JoinAll] SingleNode<TopPanelComponent> topPanel) =>
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.ShellItemsHeaderText,
                weaponMarketItem.descriptionItem.name);

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListGraffitiItemNode itemNode,
            [JoinAll] GarageItemsScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel) =>
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.GraffitiItemsHeaderText);

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, SingleNode<SkinItemComponent> itemNode,
            [JoinByParentGroup] WeaponMarketItemNode weaponMarketItem, [JoinAll] GarageItemsScreenNode screen,
            [JoinAll] SingleNode<TopPanelComponent> topPanel) =>
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.WeaponSkinItemsHeaderText,
                weaponMarketItem.descriptionItem.name);

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, SingleNode<SkinItemComponent> itemNode,
            [JoinByParentGroup] HullMarketItemNode hullMarketItemNode, [JoinAll] GarageItemsScreenNode screen,
            [JoinAll] SingleNode<TopPanelComponent> topPanel) =>
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.HullSkinItemsHeaderText,
                hullMarketItemNode.descriptionItem.name);

        [OnEventFire]
        public void ClearItemsList(NodeRemoveEvent e, ScreenNode screenNode) {
            SimpleHorizontalListComponent componentInChildrenIncludeInactive =
                screenNode.screen.GetComponentInChildrenIncludeInactive<SimpleHorizontalListComponent>();

            componentInChildrenIncludeInactive.ClearItems();
        }

        [OnEventFire]
        public void SelectItem(ListItemSelectedEvent e, GarageItemNode item, [JoinAll] ScreenNode screenNode,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode) =>
            selectedItemNode.component.SelectedItem = item.Entity;

        [OnEventFire]
        public void UpdateHeader(ListItemSelectedEvent e, GarageListNotChildItemNode item,
            [JoinAll] SingleNode<TopPanelComponent> topPanel) =>
            topPanel.component.CurrentHeader = item.descriptionItem.name;

        [OnEventFire]
        public void SetNameItem(NodeAddedEvent e, GarageListItemNode listItemNode) {
            DescriptionItemComponent descriptionItem = listItemNode.descriptionItem;
            listItemNode.garageListItemContent.Header.text = descriptionItem.name;
        }

        [OnEventFire]
        public void SetImageItem(NodeAddedEvent e, GarageListImagedItemNode listItemNode) {
            string spriteUid = listItemNode.imageItem.SpriteUid;

            if (!string.IsNullOrEmpty(spriteUid)) {
                listItemNode.garageListItemContent.SetImage(spriteUid);
            }
        }

        [OnEventFire]
        public void ResetNonImagedItem(NodeAddedEvent e, GarageListNonImagedMarketItemNode listItemNode,
            [JoinByParentGroup] DefaultSkinNode skin) {
            string spriteUid = skin.imageItem.SpriteUid;
            listItemNode.garageListItemContent.SetImage(spriteUid);
        }

        [OnEventFire]
        public void ResetNonImagedItem(NodeAddedEvent e, GarageListNonImagedUserItemNode listItemNode,
            [JoinByParentGroup] MountedSkinNode skin) {
            string spriteUid = skin.imageItem.SpriteUid;
            listItemNode.garageListItemContent.SetImage(spriteUid);
        }

        [OnEventFire]
        public void SetDescriptionItem(ListItemSelectedEvent e, GarageItemNode item, [JoinAll] ScreenNode screenNode) =>
            screenNode.displayDescriptionItem.SetDescription(item.descriptionItem.description);

        [OnEventFire]
        public void SetPrice(NodeAddedEvent e, GarageListItemPriceNode item) => ScheduleEvent(new SetPriceEvent {
                Price = item.priceItem.Price,
                XPrice = item.xPriceItem.Price
            },
            item);

        [OnEventFire]
        public void SetUpgradeLevel(NodeAddedEvent e, GarageListItemUpgradeNode item) {
            item.garageListItemContent.ProficiencyLevel.text = item.proficiencyLevelItem.Level.ToString();
            float num = item.experienceToLevelUpItem.FinalLevelExperience - item.experienceToLevelUpItem.InitLevelExperience;
            float num2 = num - item.experienceToLevelUpItem.RemainingExperience;
            float progressValue = num2 / num;
            item.garageListItemContent.ProgressBar.ProgressValue = progressValue;

            item.garageListItemContent.Arrow.gameObject.SetActive(
                item.proficiencyLevelItem.Level > item.upgradeLevelItem.Level);
        }

        [OnEventFire]
        public void EnablePrice(NodeAddedEvent e, GarageListItemPriceNode item) {
            if (item.priceItem.IsBuyable) {
                item.garageListItemContent.PriceGameObject.SetActive(true);
            } else if (item.xPriceItem.IsBuyable) {
                item.garageListItemContent.XPriceGameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void EnableUpgrade(NodeAddedEvent e, GarageListUserUpgradeItemNode item) =>
            item.garageListItemContent.UpgradeGameObject.SetActive(true);

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public DisplayDescriptionItemComponent displayDescriptionItem;
            public ItemsListScreenComponent itemsListScreen;

            public ScreenComponent screen;
        }

        public class MarketItem : Node {
            public MarketItemComponent marketItem;

            public MarketItemGroupComponent marketItemGroup;

            public OrderItemComponent orderItem;
        }

        public class UserItem : Node {
            public MarketItemGroupComponent marketItemGroup;

            public OrderItemComponent orderItem;
            public UserItemComponent userItem;
        }

        public class GarageItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;
        }

        [Not(typeof(ShellItemComponent))]
        public class NotShellGarageItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;
        }

        public class GarageItemsScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public GarageItemsScreenTextComponent garageItemsScreenText;
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

        public class GarageListItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public GarageListItemContentComponent garageListItemContent;
        }

        [Not(typeof(GraffitiItemComponent))]
        [Not(typeof(SkinItemComponent))]
        [Not(typeof(ShellItemComponent))]
        public class GarageListNotChildItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public GarageListItemContentComponent garageListItemContent;
        }

        public class GarageListGraffitiItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public GarageItemComponent garageItem;

            public GarageListItemContentComponent garageListItemContent;
            public GraffitiItemComponent graffitiItem;
        }

        public class GarageListShellItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public GarageItemComponent garageItem;

            public GarageListItemContentComponent garageListItemContent;
            public ShellItemComponent shellItem;
        }

        [Not(typeof(ImageItemComponent))]
        public class GarageListNonImagedItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public GarageListItemContentComponent garageListItemContent;
        }

        public class GarageListNonImagedUserItemNode : GarageListNonImagedItemNode {
            public UserItemComponent userItem;
        }

        public class GarageListNonImagedMarketItemNode : GarageListNonImagedItemNode {
            public MarketItemComponent marketItem;
        }

        public class GarageListImagedItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public GarageListItemContentComponent garageListItemContent;

            public ImageItemComponent imageItem;
        }

        public class ItemsListNode : Node {
            public ItemsListForViewComponent itemsListForView;

            public ScreenGroupComponent screenGroup;
        }

        public class GarageListItemPriceNode : Node {
            public GarageListItemContentComponent garageListItemContent;
            public MarketItemComponent marketItem;

            public PriceItemComponent priceItem;

            public PriceLabelComponent priceLabel;

            public XPriceItemComponent xPriceItem;

            public XPriceLabelComponent xPriceLabel;
        }

        public class GarageListItemUpgradeNode : Node {
            public ExperienceToLevelUpItemComponent experienceToLevelUpItem;
            public GarageListItemContentComponent garageListItemContent;

            public ProficiencyLevelItemComponent proficiencyLevelItem;

            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        [Not(typeof(UpgradeLevelItemComponent))]
        public class GarageListUserItemNotUpgradeNode : Node {
            public GarageListItemContentComponent garageListItemContent;
            public UserItemComponent userItem;
        }

        public class GarageListUserUpgradeItemNode : Node {
            public GarageListItemContentComponent garageListItemContent;

            public UpgradeLevelItemComponent upgradeLevelItem;
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
    }
}