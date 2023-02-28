using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientPayment.Impl;

public class GoodsPriceSystem : ECSSystem {
    [OnEventFire]
    public void UpdateGoodsPrice(UpdateGoodsPriceEvent e, GoodsNode goods) {
        if (goods.Entity.HasComponent<GoodsPriceComponent>()) {
            GoodsPriceComponent component = goods.Entity.GetComponent<GoodsPriceComponent>();
            component.Currency = e.Currency;
            component.Price = e.Price;
        } else {
            GoodsPriceComponent goodsPriceComponent = new();
            goodsPriceComponent.Currency = e.Currency;
            goodsPriceComponent.Price = e.Price;
            GoodsPriceComponent component2 = goodsPriceComponent;
            goods.Entity.AddComponent(component2);
        }

        if (goods.Entity.HasComponent<SpecialOfferComponent>()) {
            SpecialOfferComponent component3 = goods.Entity.GetComponent<SpecialOfferComponent>();
            component3.Discount = e.DiscountCoeff * 100f;
        }

        ScheduleEvent<GoodsChangedEvent>(goods.Entity);
    }

    public class GoodsNode : Node {
        public GoodsComponent goods;
    }
}