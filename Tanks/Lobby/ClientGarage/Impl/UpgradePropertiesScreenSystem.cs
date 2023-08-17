using System.Collections.Generic;
using System.Linq;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientPaymentGUI.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradePropertiesScreenSystem : ECSSystem {
        [OnEventFire]
        public void ShowScreen(ButtonClickEvent e, SingleNode<UpgradeButtonComponent> button,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode) {
            ShowScreenLeftEvent<ItemPropertiesScreenComponent> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(selectedItemNode.component.SelectedItem, false);
            ScheduleEvent(showScreenLeftEvent, button);
        }

        [OnEventFire]
        public void ShowUpgradePart(NodeAddedEvent e, ScreenNode screen, [JoinByScreen] [Context] ItemNode item) {
            screen.Entity.AddComponent(
                new ScreenHeaderTextComponent(screen.itemUpgradeScreenText.UpgradeText +
                                              " " +
                                              item.descriptionItem.name.ToUpper()));

            ScheduleEvent<HideItemPropertiesEvent>(screen);
            ScheduleEvent<ShowItemPropertiesEvent>(item);
            ScheduleEvent(new UpdateItemPropertiesEvent(item.upgradeLevelItem.Level, item.proficiencyLevelItem.Level), item);
            screen.itemPropertiesScreen.UpgradeButton.SetActive(true);
            screen.itemPropertiesScreen.itemAttribute.Show();
        }

        [OnEventFire]
        public void PopulateRewards(NodeAddedEvent e, ScreenNode screen, [JoinByScreen] [Context] ItemNode item,
            [Context] [JoinByScreen] ListNode list) {
            IList<RewardNode> list2 = Select<RewardNode>(item.Entity, typeof(ParentGroupComponent));

            if (list2.Count <= 0) {
                return;
            }

            screen.Entity.AddComponent(new SelectedItemComponent(list2[0].Entity));
            EventBuilder eventBuilder = NewEvent<ShowGarageItemsEvent>();

            foreach (RewardNode item2 in list2) {
                if (IsReward(item2.Entity) && item2.Entity != item.Entity && item.marketItemGroup.Key != item2.Entity.Id) {
                    eventBuilder.Attach(item2);
                }
            }

            eventBuilder.Schedule();
        }

        bool IsReward(Entity reward) {
            IList<MarketItemMountRestrictionsNode> list =
                Select<MarketItemMountRestrictionsNode>(reward, typeof(MarketItemGroupComponent));

            bool flag = false;

            if (list.Count > 0) {
                flag = list.Single().mountUpgradeLevelRestriction.RestrictionValue > 0;
            }

            IList<MarketItemPurchaseRestrictionsNode> list2 =
                Select<MarketItemPurchaseRestrictionsNode>(reward, typeof(MarketItemGroupComponent));

            if (list2.Count > 0) {
                flag = flag || list2.Single().purchaseUpgradeLevelRestriction.RestrictionValue > 0;
            }

            return flag;
        }

        [OnEventFire]
        public void AddUpgradeButton(NodeAddedEvent e, ButtonNode upgradeButton,
            [Context] [JoinByScreen] UpgradableItemNode item, [JoinByUser] UserNode user) =>
            UpdatePrice(upgradeButton, item);

        [OnEventFire]
        public void ChangeUserMoney(UserMoneyChangedEvent e, UserNode user, [JoinByUser] UpgradableItemNode item,
            [JoinByScreen] ButtonNode upgradeButton) => UpdatePrice(upgradeButton, item);

        void UpdatePrice(ButtonNode upgradeButton, UpgradableItemNode item) {
            long price = item.nextUpgradePrice.Price;
            upgradeButton.priceButton.Price = price;

            ScheduleEvent(new SetPriceEvent {
                    Price = price
                },
                upgradeButton);

            UpdateColorButton(item, upgradeButton);
        }

        static void UpdateColorButton(UpgradableItemNode item, ButtonNode upgradeButton) {
            if (PropertyUiUtils.IsOverUpgradeItem(item.upgradeLevelItem, item.proficiencyLevelItem)) {
                upgradeButton.buyButton.gameObject.GetComponent<ColorButtonController>().SetColor(1);
            } else {
                upgradeButton.buyButton.gameObject.GetComponent<ColorButtonController>().SetDefault();
            }
        }

        [OnEventFire]
        public void HideUpgradeButton(NodeAddedEvent e, MaxUpgradableLevelItemNode item,
            [Context] [JoinByScreen] ScreenNode screenNode, [JoinByScreen] [Context] ButtonNode upgradeButton) {
            ScheduleEvent(new UpdateItemPropertiesEvent(item.upgradeLevelItem.Level, item.proficiencyLevelItem.Level), item);
            upgradeButton.buyButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void SetUpgradeColorForProperties(UpdateItemPropertiesEvent e, Node item,
            [JoinByScreen] SingleNode<ItemAttributesComponent> attributes,
            [JoinAll] ICollection<SingleNode<PropertyUIComponent>> properties,
            [JoinAll] ICollection<SingleNode<GarageListItemContentComponent>> rewards) {
            foreach (SingleNode<PropertyUIComponent> property in properties) {
                property.component.ShowNextUpgradeValue = attributes.component.ShowNextUpgradeValue;

                property.component.SetNextValueColor(e.Level < e.Proficiency ? attributes.component.nextValueUpgradeColor
                                                         : attributes.component.nextValueOverUpgradeColor);
            }

            foreach (SingleNode<GarageListItemContentComponent> reward in rewards) {
                reward.component.SetUpgradeColor(e.Level < e.Proficiency ? attributes.component.nextValueUpgradeColor
                                                     : attributes.component.nextValueOverUpgradeColor);
            }
        }

        [OnEventFire]
        public void HideUpgradeButton(NodeAddedEvent e, BuyableItemNode item, ButtonNode upgradeButton) =>
            upgradeButton.buyButton.gameObject.SetActive(false);

        [OnEventFire]
        public void HideUpgradeButton(NodeAddedEvent e, SupplyItemNode item, ButtonNode upgradeButton) =>
            upgradeButton.buyButton.gameObject.SetActive(false);

        [OnEventFire]
        public void UpdatePrice(ItemUpgradedEvent e, UpgradableItemNode item, [JoinByScreen] ButtonNode upgradeButton,
            UpgradableItemNode item2, [JoinByUser] UserNode user) => UpdatePrice(upgradeButton, item);

        [OnEventFire]
        public void Upgrade(ConfirmButtonClickEvent e, ButtonNode button, [JoinByScreen] UpgradeLevelNode levelText,
            [JoinByScreen] UpgradableItemNode item, [JoinByScreen] ScreenNode screen, [JoinAll] UserNode user,
            [JoinAll] SingleNode<DialogsComponent> dialogs) {
            if (user.userMoney.Money < button.priceLabel.Price) {
                dialogs.component.Get<NotEnoughCrystalsWindow>().ShowForCrystals(user.Entity,
                    screen.Entity,
                    button.priceLabel.Price - user.userMoney.Money);

                return;
            }

            ScheduleEvent(new UpgradeItemEvent(button.priceLabel.Price), item);
            levelText.itemAttributes.AnimateUpgrade(item.upgradeLevelItem.Level + 1);

            ScheduleEvent(new UpdateItemPropertiesEvent(item.upgradeLevelItem.Level + 1, item.proficiencyLevelItem.Level),
                item);
        }

        [OnEventFire]
        public void MaxUpgradeLevelItem(NodeAddedEvent e, SingleNode<UpgradeMaxLevelItemComponent> item,
            [JoinByScreen] [Context] ButtonNode upgradeButton) => upgradeButton.buyButton.gameObject.SetActive(false);

        [OnEventFire]
        public void WarningText(NodeAddedEvent e, WarningTextNode warningText, [JoinByScreen] UpgradableItemNode item) =>
            UpdateVisibilityWarningText(warningText, item);

        static void UpdateVisibilityWarningText(WarningTextNode warningText, UpgradableItemNode item) =>
            warningText.warningOverUpgrade.gameObject.SetActive(
                PropertyUiUtils.IsOverUpgradeItem(item.upgradeLevelItem, item.proficiencyLevelItem));

        [OnEventFire]
        public void HideWarningText(NodeAddedEvent e, WarningTextNode warningText, [JoinByScreen] BuyableItemNode item) =>
            warningText.warningOverUpgrade.gameObject.SetActive(false);

        [OnEventFire]
        public void HideWarningText(NodeAddedEvent e, WarningTextNode warningText, [JoinByScreen] SupplyItemNode item) =>
            warningText.warningOverUpgrade.gameObject.SetActive(false);

        [OnEventFire]
        public void UpdateWarning(ItemUpgradedEvent e, UpgradableItemNode item,
            [JoinByScreen] WarningTextNode warningText) => UpdateVisibilityWarningText(warningText, item);

        public class ButtonNode : Node {
            public BuyButtonComponent buyButton;
            public PriceButtonComponent priceButton;

            public PriceLabelComponent priceLabel;

            public ScreenGroupComponent screenGroup;

            public UpgradeButtonComponent upgradeButton;
        }

        public class UpgradeLevelNode : Node {
            public ItemAttributesComponent itemAttributes;

            public ScreenGroupComponent screenGroup;
        }

        public class ItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public MarketItemGroupComponent marketItemGroup;

            public ProficiencyLevelItemComponent proficiencyLevelItem;

            public ScreenGroupComponent screenGroup;

            public UpgradeLevelItemComponent upgradeLevelItem;

            public UserGroupComponent userGroup;
        }

        public class UpgradableItemNode : ItemNode {
            public NextUpgradePriceComponent nextUpgradePrice;
        }

        public class MaxUpgradableLevelItemNode : ItemNode {
            public UpgradeMaxLevelItemComponent upgradeMaxLevelItem;
        }

        public class BuyableItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MarketItemComponent marketItem;

            public ScreenGroupComponent screenGroup;
        }

        public class SupplyItemNode : Node {
            public ScreenGroupComponent screenGroup;

            public SupplyItemComponent supplyItem;
            public UserItemComponent userItem;
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public DisplayDescriptionItemComponent displayDescriptionItem;
            public ItemPropertiesScreenComponent itemPropertiesScreen;

            public ItemsListScreenComponent itemsListScreen;

            public ItemUpgradeScreenTextComponent itemUpgradeScreenText;

            public ScreenComponent screen;

            public ScreenGroupComponent screenGroup;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;

            public UserMoneyComponent userMoney;
        }

        public class RewardNode : Node {
            public GarageItemComponent garageItem;

            public MarketItemGroupComponent marketItemGroup;
        }

        public class MarketItemMountRestrictionsNode : Node {
            public MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction;
        }

        public class MarketItemPurchaseRestrictionsNode : Node {
            public PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction;
        }

        public class WarningTextNode : Node {
            public ScreenGroupComponent screenGroup;
            public WarningOverUpgradeComponent warningOverUpgrade;
        }

        public class ListNode : Node {
            public ScreenGroupComponent screenGroup;
            public SimpleHorizontalListComponent simpleHorizontalList;
        }
    }
}