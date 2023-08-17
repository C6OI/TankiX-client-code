using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplayBuyButtonSystem : ECSSystem {
        [OnEventFire]
        public void LocalizeBuyButton(NodeAddedEvent e, BuyButtonNode node) {
            node.confirmButton.ButtonText = node.buyButtonConfirmWithPriceLocalizedText.BuyText +
                                            " " +
                                            node.buyButtonConfirmWithPriceLocalizedText.ForText;

            node.confirmButton.CancelText = node.buyButtonConfirmWithPriceLocalizedText.CancelText;
            node.confirmButton.ConfirmText = node.buyButtonConfirmWithPriceLocalizedText.ConfirmText;
        }

        [OnEventFire]
        public void HideBuyButton(ListItemSelectedEvent e, UserItemNode item, [JoinAll] ScreenNode screen) {
            HideBuyButton(screen);
            HideXBuyButton(screen);
        }

        [OnEventFire]
        public void SwitchBuyButton(ListItemSelectedEvent e, BuyableMarketItemNode item, [JoinAll] ScreenNode screen) {
            CheckMarketItemRestrictionsEvent checkMarketItemRestrictionsEvent = new();
            ScheduleEvent(checkMarketItemRestrictionsEvent, item);

            if (checkMarketItemRestrictionsEvent.RestrictedByRank ||
                checkMarketItemRestrictionsEvent.RestrictedByUpgradeLevel) {
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
                    Price = priceItem.Price
                },
                buyButton.GetComponent<EntityBehaviour>().Entity);

            buyButton.GetComponent<PriceButtonComponent>().Price = priceItem.Price;
        }

        void ShowXBuyButton(XPriceItemComponent priceItem, ScreenNode screenNode) {
            GameObject xBuyButton = screenNode.garageItemsScreen.XBuyButton;
            xBuyButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(xBuyButton.gameObject);

            ScheduleEvent(new SetPriceEvent {
                    XPrice = priceItem.Price
                },
                xBuyButton.GetComponent<EntityBehaviour>().Entity);

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
            public UserItemComponent userItem;
        }

        public class BuyButtonNode : Node {
            public BuyButtonComponent buyButton;

            public BuyButtonConfirmWithPriceLocalizedTextComponent buyButtonConfirmWithPriceLocalizedText;

            public ConfirmButtonComponent confirmButton;
        }
    }
}