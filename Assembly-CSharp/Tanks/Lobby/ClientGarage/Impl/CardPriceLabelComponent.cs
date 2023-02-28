using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CardPriceLabelComponent : UIBehaviour, Component {
        [SerializeField] Text[] resourceCountTexts;

        [SerializeField] GameObject[] spacingObjects;

        [SerializeField] Color textColorWhenResourceEnough = Color.green;

        [SerializeField] Color textColorWhenResourceNotEnough = Color.red;

        readonly long[] counts = new long[1];

        readonly int[] prices = new int[1];

        public bool EnoughCards { get; private set; }

        void SetPrice(long type, long price) {
            int num = 0;
            prices[num] = (int)price;
            EnoughCards = EnoughCards && prices[num] <= counts[num];
            resourceCountTexts[num].text = GetText(prices[num], counts[num]);
            resourceCountTexts[num].color = GetColor(prices[num], counts[num]);
            resourceCountTexts[num].gameObject.SetActive(true);
            spacingObjects[num].SetActive(true);
        }

        public void SetPrices(ModuleCardsCompositionComponent moduleResourcesComponent) {
            EnoughCards = true;

            for (int i = 0; i < prices.Length; i++) {
                resourceCountTexts[i].gameObject.SetActive(false);
                spacingObjects[i].SetActive(false);
                prices[i] = 0;
            }

            SetPrice(123123L, moduleResourcesComponent.CraftPrice.Cards);
        }

        public void SetUserCardsCount(long count) {
            int num = 0;
            counts[num] = count;
            resourceCountTexts[num].text = GetText(prices[num], counts[num]);
            resourceCountTexts[num].color = GetColor(prices[num], counts[num]);
        }

        public void SetRefund(long type, long count) {
            int num = (byte)type;
            counts[num] = count;
            resourceCountTexts[num].text = count.ToString();
            resourceCountTexts[num].color = textColorWhenResourceEnough;
            resourceCountTexts[num].gameObject.SetActive(true);
            spacingObjects[num].SetActive(true);
        }

        string GetText(int price, long count) => count + " / " + price;

        Color GetColor(int price, long count) => count < price ? textColorWhenResourceNotEnough : textColorWhenResourceEnough;
    }
}