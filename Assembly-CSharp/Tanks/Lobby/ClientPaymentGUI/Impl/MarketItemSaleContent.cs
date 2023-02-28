using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MarketItemSaleContent : DealItemContent {
        [SerializeField] PaletteColorField greyColor;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public virtual string Price { get; set; }

        void SetPrice(string priceStr, string currency) {
            price.text = FormatPrice(priceStr, currency);
        }

        string FormatPrice(string priceStr, string currency) => Price.Replace("{PRICE}", priceStr).Replace("{CURRENCY}", currency);

        protected override void FillFromEntity(Entity entity) {
            if (entity.HasComponent<ImageItemComponent>()) {
                string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                banner.SpriteUid = spriteUid;
            }

            title.text = MarketItemNameLocalization.Instance.GetCategoryName(entity) + " \"";
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(entity);

            if (item != null) {
                title.text += MarketItemNameLocalization.Instance.GetGarageItemName(item);
            } else if (entity.HasComponent<DescriptionItemComponent>()) {
                DescriptionItemComponent component = entity.GetComponent<DescriptionItemComponent>();
                title.text += component.Name;
            }

            title.text += "\"";
            XPriceItemComponent component2 = entity.GetComponent<XPriceItemComponent>();
            string text = component2.Price.ToStringSeparatedByThousands();

            if (component2.Price < component2.OldPrice) {
                text += string.Format(" <s><#{0}>{1}</color></s>", greyColor.Color.ToHexString(), component2.OldPrice.ToStringSeparatedByThousands());
            }

            SetPrice(text, "<sprite=9>");
            EndDate = entity.GetComponent<MarketItemSaleComponent>().endDate;

            if (EndDate.UnityTime != 0f) {
                TextTimerComponent component3 = GetComponent<TextTimerComponent>();
                component3.EndDate = EndDate;
                component3.enabled = true;
            }

            base.FillFromEntity(entity);
        }
    }
}