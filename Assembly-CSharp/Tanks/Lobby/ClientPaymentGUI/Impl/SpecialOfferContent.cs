using System.Text;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientPayment.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OrderItemComponent = Tanks.Lobby.ClientPayment.Impl.OrderItemComponent;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class SpecialOfferContent : DealItemContent {
        public TextMeshProUGUI oldPrice;

        public TextMeshProUGUI items;

        public LocalizedField specialOfferEmptyRewardMessage;

        [SerializeField] Image saleImage;

        [SerializeField] TextMeshProUGUI saleText;

        [SerializeField] Image titleStripes;

        [SerializeField] TextMeshProUGUI timer;

        string cachedCurrency;

        double cachedPrice;

        public virtual string Price { get; set; }

        string FormatPrice(double price, string currency) => Price.Replace("{PRICE}", price.ToStringSeparatedByThousands()).Replace("{CURRENCY}", currency);

        public void SetSaleText(string text) {
            saleText.text = text;
        }

        protected override void FillFromEntity(Entity entity) {
            GoodsPriceComponent component = entity.GetComponent<GoodsPriceComponent>();

            if (!(component.Currency == cachedCurrency) || component.Price != cachedPrice) {
                cachedCurrency = component.Currency;
                cachedPrice = component.Price;
                SpecialOfferContentLocalizationComponent component2 = entity.GetComponent<SpecialOfferContentLocalizationComponent>();
                description.text = component2.Description;
                title.text = component2.Title;
                banner.SpriteUid = component2.SpriteUid;
                order = entity.GetComponent<OrderItemComponent>().Index;
                SpecialOfferContentComponent component3 = entity.GetComponent<SpecialOfferContentComponent>();
                double num = component.Price;

                if (component3.SalePercent == 0) {
                    oldPrice.gameObject.SetActive(true);
                    oldPrice.text = FormatPrice(num, cachedCurrency);
                    num = component.Round(num * (100 - component3.SalePercent) / 100.0);
                    saleImage.gameObject.SetActive(false);
                } else {
                    oldPrice.gameObject.SetActive(false);
                    saleImage.gameObject.SetActive(true);
                    saleText.text = "-" + component3.SalePercent + "%";
                }

                price.text = FormatPrice(num, cachedCurrency);

                if (component3.HighlightTitle) {
                    title.faceColor = new Color32(byte.MaxValue, 188, 9, byte.MaxValue);
                    titleStripes.gameObject.SetActive(true);
                }

                EndDate = entity.GetComponent<SpecialOfferEndTimeComponent>().EndDate;
                TextTimerComponent component4 = GetComponent<TextTimerComponent>();
                component4.EndDate = EndDate;
                component4.enabled = true;
                ItemsPackFromConfigComponent component5 = entity.GetComponent<ItemsPackFromConfigComponent>();

                if (component3.ShowItemsList) {
                    StringBuilder stringBuilder = buildComment(entity, component5);
                    items.text = stringBuilder.ToString();
                } else {
                    Vector3 localPosition = timer.transform.localPosition;
                    timer.transform.localPosition = new Vector3(localPosition.x, items.transform.localPosition.y, localPosition.z);
                    items.gameObject.SetActive(false);
                }

                base.FillFromEntity(entity);
            }
        }

        StringBuilder buildComment(Entity entity, ItemsPackFromConfigComponent itemsPackFromConfig) {
            StringBuilder stringBuilder = new();

            if (itemsPackFromConfig.Pack.Count > 0) {
                int num = 0;
                stringBuilder.Append("* —");
                bool flag = true;

                foreach (long item in itemsPackFromConfig.Pack) {
                    ItemInMarketRequestEvent itemInMarketRequestEvent = new();
                    this.SendEvent(itemInMarketRequestEvent, entity);

                    if (itemInMarketRequestEvent.marketItems.ContainsKey(item)) {
                        if (!flag) {
                            stringBuilder.Append(", ");
                        }

                        flag = false;
                        stringBuilder.Append(itemInMarketRequestEvent.marketItems[item]);
                        num++;
                    }
                }

                if (num == 0) {
                    stringBuilder.Append(specialOfferEmptyRewardMessage);
                }
            }

            return stringBuilder;
        }
    }
}