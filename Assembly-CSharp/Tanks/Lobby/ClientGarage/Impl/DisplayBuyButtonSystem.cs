using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplayBuyButtonSystem : ECSSystem {
        [OnEventFire]
        public void LocalizeBuyButton(NodeAddedEvent e, BuyButtonNode node) {
            node.confirmButton.ButtonText = node.buyButtonConfirmWithPriceLocalizedText.BuyText + " " + node.buyButtonConfirmWithPriceLocalizedText.ForText;
            node.confirmButton.CancelText = node.buyButtonConfirmWithPriceLocalizedText.CancelText;
            node.confirmButton.ConfirmText = node.buyButtonConfirmWithPriceLocalizedText.ConfirmText;
        }

        [OnEventFire]
        public void HideBuyButton(ListItemSelectedEvent e, UserItemNode item, [JoinAll] ScreenNode screen) {
            HideBuyButton(screen);
            HideXBuyButton(screen);
        }

        [OnEventFire]
        public void PriceChanged(PriceChangedEvent e, BuyableMarketItemNode item) {
            item.priceItem.OldPrice = (int)e.OldPrice;
            item.priceItem.Price = (int)e.Price;
            item.xPriceItem.OldPrice = (int)e.OldXPrice;
            item.xPriceItem.Price = (int)e.XPrice;
        }

        [OnEventComplete]
        public void PriceChanged(PriceChangedEvent e, BuyableMarketItemNode item, [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem) {
            bool flag = item.Entity.Equals(selectedItem.component.SelectedItem);
            Log.InfoFormat("PriceChanged {0} item={1} itemIsSelected={2}", e, item, flag);
            GameObject buyButton = screen.garageItemsScreen.BuyButton;
            GameObject xBuyButton = screen.garageItemsScreen.XBuyButton;
            buyButton.GetComponent<PriceButtonComponent>().Price = e.Price;
            xBuyButton.GetComponent<PriceButtonComponent>().Price = e.XPrice;

            if (flag) {
                if (buyButton.activeSelf) {
                    Entity entity = buyButton.GetComponent<EntityBehaviour>().Entity;

                    ScheduleEvent(new SetPriceEvent {
                        Price = e.Price,
                        OldPrice = e.OldPrice,
                        XPrice = e.XPrice,
                        OldXPrice = e.OldXPrice
                    }, entity);
                }

                if (xBuyButton.activeSelf) {
                    Entity entity2 = xBuyButton.GetComponent<EntityBehaviour>().Entity;

                    ScheduleEvent(new SetPriceEvent {
                        Price = e.Price,
                        OldPrice = e.OldPrice,
                        XPrice = e.XPrice,
                        OldXPrice = e.OldXPrice
                    }, entity2);
                }
            }
        }

        [OnEventFire]
        public void SwitchBuyButton(ListItemSelectedEvent e, BuyableMarketItemNode item, [JoinByParentGroup] ICollection<UserItemNode> parentUserItem, [JoinAll] ScreenNode screen) {
            CheckMarketItemRestrictionsEvent checkMarketItemRestrictionsEvent = new();
            ScheduleEvent(checkMarketItemRestrictionsEvent, item);

            if (checkMarketItemRestrictionsEvent.RestrictedByRank || checkMarketItemRestrictionsEvent.RestrictedByUpgradeLevel) {
                HideBuyButton(screen);
                HideXBuyButton(screen);
                return;
            }

            if (item.priceItem.IsBuyable) {
                ShowBuyButton(item.priceItem, screen);
            } else {
                HideBuyButton(screen);
            }

            if (item.xPriceItem.IsBuyable) {
                ShowXBuyButton(item.xPriceItem, screen);
            } else {
                HideXBuyButton(screen);
            }
        }

        void ShowBuyButton(PriceItemComponent priceItem, ScreenNode screenNode) {
            GameObject buyButton = screenNode.garageItemsScreen.BuyButton;
            buyButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(buyButton.gameObject);

            ScheduleEvent(new SetPriceEvent {
                Price = priceItem.Price,
                OldPrice = priceItem.OldPrice
            }, buyButton.GetComponent<EntityBehaviour>().Entity);

            buyButton.GetComponent<PriceButtonComponent>().Price = priceItem.Price;
        }

        void ShowXBuyButton(XPriceItemComponent priceItem, ScreenNode screenNode) {
            GameObject xBuyButton = screenNode.garageItemsScreen.XBuyButton;
            xBuyButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(xBuyButton.gameObject);

            ScheduleEvent(new SetPriceEvent {
                XPrice = priceItem.Price,
                OldXPrice = priceItem.OldPrice
            }, xBuyButton.GetComponent<EntityBehaviour>().Entity);

            xBuyButton.GetComponent<PriceButtonComponent>().Price = priceItem.Price;
        }

        void HideBuyButton(ScreenNode screenNode) {
            GameObject buyButton = screenNode.garageItemsScreen.BuyButton;
            buyButton.SetActive(false);
        }

        void HideXBuyButton(ScreenNode screenNode) {
            GameObject xBuyButton = screenNode.garageItemsScreen.XBuyButton;
            xBuyButton.SetActive(false);
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenComponent garageItemsScreen;

            public ScreenComponent screen;
        }

        public class BuyableMarketItemNode : Node {
            public MarketItemComponent marketItem;

            public PriceItemComponent priceItem;

            public XPriceItemComponent xPriceItem;
        }

        public class UserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public UserItemComponent userItem;
        }

        public class BuyButtonNode : Node {
            public BuyButtonComponent buyButton;

            public BuyButtonConfirmWithPriceLocalizedTextComponent buyButtonConfirmWithPriceLocalizedText;

            public ConfirmButtonComponent confirmButton;
        }
    }
}