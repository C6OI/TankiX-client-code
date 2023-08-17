using Lobby.ClientControls.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientPayment.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ExchangeCrystalsContent : ListItemEntityContent {
        [SerializeField] Text priceText;

        [SerializeField] Text crystalsText;

        Entity entity;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        protected override void FillFromEntity(Entity entity) {
            GoodsXPriceComponent component = entity.GetComponent<GoodsXPriceComponent>();
            priceText.text = component.Price.ToStringSeparatedByThousands();
            GameCurrencyPackComponent component2 = entity.GetComponent<GameCurrencyPackComponent>();
            crystalsText.text = component2.Amount.ToStringSeparatedByThousands();
        }
    }
}