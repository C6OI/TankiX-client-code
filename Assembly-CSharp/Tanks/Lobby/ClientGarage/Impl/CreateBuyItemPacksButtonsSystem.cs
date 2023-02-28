using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CreateBuyItemPacksButtonsSystem : ECSSystem {
        [OnEventFire]
        public void ClearBuyButtons(ListItemSelectedEvent e, GarageItemNode item, [JoinAll] ScreenNode screen) {
            screen.buyItemPacksButtons.SetBuyButtonsInactive();
        }

        [OnEventComplete]
        public void CreateBuyButtons(ListItemSelectedEvent e, GarageItemNode item, [JoinByMarketItem] MarketItemPackNode marketItem, [JoinAll] ScreenNode screen) {
            int index = 0;
            index = CreateButtons(marketItem.packPrice.PackPrice.Keys.ToList(), screen, index, true, false);
            CreateButtons(marketItem.packPrice.PackXPrice.Keys.ToList(), screen, index, false, true);
        }

        [OnEventComplete]
        public void CreateBuyButtons(ListItemSelectedEvent e, GarageItemNode item, [JoinByMarketItem] MarketItemNode marketItem, [JoinAll] ScreenNode screen) {
            int index = 0;
            index = CreateButtons(marketItem.itemPacks.ForPrice, screen, index, true, false);
            CreateButtons(marketItem.itemPacks.ForXPrice, screen, index, false, true);
        }

        int CreateButtons(List<int> packs, ScreenNode screen, int index, bool priceActivity, bool xPriceActivity) {
            if (packs != null && packs.Any()) {
                packs.ForEach(delegate(int packSize) {
                    if (packSize > 0) {
                        ActivateButton(screen, packSize, index, priceActivity, xPriceActivity);
                        index++;
                    }
                });
            }

            return index;
        }

        void ActivateButton(ScreenNode screen, int packSize, int index, bool priceActivity, bool xPriceActivity) {
            EntityBehaviour entityBehaviour = screen.buyItemPacksButtons.BuyButtons[index];
            entityBehaviour.GetComponent<ItemPackButtonComponent>().Count = packSize;
            entityBehaviour.GetComponent<UniversalPriceButtonComponent>().PriceActivity = priceActivity;
            entityBehaviour.GetComponent<UniversalPriceButtonComponent>().XPriceActivity = xPriceActivity;
            entityBehaviour.gameObject.SetActive(true);
            NewEvent<SetBuyItemPackButtonInfoEvent>().Attach(entityBehaviour.Entity).Schedule();
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public BuyItemPacksButtonsComponent buyItemPacksButtons;

            public ScreenComponent screen;
        }

        public class GarageItemNode : Node {
            public GarageListItemContentComponent garageListItemContent;

            public MarketItemGroupComponent marketItemGroup;
        }

        [Not(typeof(PackPriceComponent))]
        [Not(typeof(HiddenInGarageItemComponent))]
        public class MarketItemNode : Node {
            public ItemPacksComponent itemPacks;

            public MarketItemComponent marketItem;

            public PriceItemComponent priceItem;

            public XPriceItemComponent xPriceItem;
        }

        [Not(typeof(HiddenInGarageItemComponent))]
        public class MarketItemPackNode : Node {
            public ItemPacksComponent itemPacks;

            public MarketItemComponent marketItem;

            public PackPriceComponent packPrice;

            public PriceItemComponent priceItem;

            public XPriceItemComponent xPriceItem;
        }
    }
}