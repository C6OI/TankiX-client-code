using System.Collections.Generic;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientPayment.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [RequireComponent(typeof(StarterPackTimerComponent))]
    public class StarterPackScreenUIComponent : PurchaseItemComponent {
        [SerializeField] StarterPackElementComponent elementPrefab;

        [SerializeField] RectTransform mainPreviewContainer;

        [SerializeField] RectTransform previewContainer;

        [SerializeField] TextMeshProUGUI title;

        [SerializeField] TextMeshProUGUI description;

        [SerializeField] TextMeshProUGUI hurryUp;

        [SerializeField] TextMeshProUGUI newPrice;

        [SerializeField] TextMeshProUGUI mainItemDescription;

        Entity packEntity;

        public Entity PackEntity {
            get => packEntity;
            set {
                packEntity = value;

                if (value != null) {
                    UpdateTimer();
                    ClearElements();
                    UpdateElements(packEntity);
                }
            }
        }

        void UpdateTimer() {
            long num = (long)(packEntity.GetComponent<SpecialOfferEndTimeComponent>().EndDate - Date.Now);
            StarterPackTimerComponent component = GetComponent<StarterPackTimerComponent>();
            component.RunTimer(num);
            component.onTimerExpired = Close;
        }

        void UpdateElements(Entity entity) {
            SpecialOfferContentLocalizationComponent component = entity.GetComponent<SpecialOfferContentLocalizationComponent>();
            title.text = component.Title;
            SpecialOfferScreenLocalizationComponent component2 = entity.GetComponent<SpecialOfferScreenLocalizationComponent>();
            hurryUp.text = component2.Footer;
            description.text = component2.Description;
            GoodsPriceComponent component3 = entity.GetComponent<GoodsPriceComponent>();
            newPrice.text = component3.Price + " " + component3.Currency;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)newPrice.rectTransform.parent.parent);
            ItemsPackFromConfigComponent component4 = entity.GetComponent<ItemsPackFromConfigComponent>();
            CountableItemsPackComponent component5 = entity.GetComponent<CountableItemsPackComponent>();
            XCrystalsPackComponent component6 = entity.GetComponent<XCrystalsPackComponent>();
            CrystalsPackComponent component7 = entity.GetComponent<CrystalsPackComponent>();
            List<long> list = new(component4.Pack);
            list.AddRange(component5.Pack.Keys);
            RequestInfoForItemsEvent requestInfoForItemsEvent = new(list);
            this.SendEvent(requestInfoForItemsEvent, entity);
            AddMainItem(requestInfoForItemsEvent.mainItemTitle, requestInfoForItemsEvent.mainItemCount, requestInfoForItemsEvent.mainItemSprite);
            mainItemDescription.text = requestInfoForItemsEvent.mainItemDescription;

            foreach (long key in component5.Pack.Keys) {
                if (requestInfoForItemsEvent.mainItemId != key) {
                    string text = requestInfoForItemsEvent.titles[key];
                    int num = component5.Pack[key];
                    string spriteUid = requestInfoForItemsEvent.previews[key];
                    bool needFrame = requestInfoForItemsEvent.rarityFrames[key];
                    ItemRarityType rarity = requestInfoForItemsEvent.rarities[key];
                    AddItem(text, num, previewContainer.transform, spriteUid, needFrame, rarity);
                }
            }

            foreach (long item in component4.Pack) {
                if (requestInfoForItemsEvent.mainItemId != item) {
                    string text2 = requestInfoForItemsEvent.titles[item];
                    string spriteUid2 = requestInfoForItemsEvent.previews[item];
                    bool needFrame2 = requestInfoForItemsEvent.rarityFrames[item];
                    ItemRarityType rarity2 = requestInfoForItemsEvent.rarities[item];
                    AddItem(text2, 0L, previewContainer.transform, spriteUid2, needFrame2, rarity2);
                }
            }

            if (component7.Total > 0 && !requestInfoForItemsEvent.mainItemCrystal) {
                AddItem(requestInfoForItemsEvent.crystalTitle, component7.Total, previewContainer.transform, requestInfoForItemsEvent.crystalSprite);
            }

            if (component6.Amount + component6.Bonus > 0 && !requestInfoForItemsEvent.mainItemXCrystal) {
                AddItem(requestInfoForItemsEvent.xCrystalTitle, component6.Amount + component6.Bonus, previewContainer.transform, requestInfoForItemsEvent.xCrystalSprite);
            }
        }

        void ClearElements() {
            mainPreviewContainer.gameObject.SetActive(false);

            if (mainPreviewContainer.childCount > 1) {
                Destroy(mainPreviewContainer.GetChild(0).gameObject);
            }

            for (int i = 0; i < previewContainer.transform.childCount; i++) {
                if (!(previewContainer.GetChild(i).GetComponent<StarterPackElementComponent>() == null)) {
                    Destroy(previewContainer.GetChild(i).gameObject);
                }
            }
        }

        void AddMainItem(string title, long count, string spriteUid, bool needFrame = false, ItemRarityType rarity = ItemRarityType.COMMON) {
            mainPreviewContainer.gameObject.SetActive(true);
            GameObject gameObject = AddItem(title, count, mainPreviewContainer, spriteUid, needFrame, rarity);
            gameObject.transform.SetAsFirstSibling();
        }

        GameObject AddItem(string title, long count, Transform parent, string spriteUid, bool needFrame = false, ItemRarityType rarity = ItemRarityType.COMMON) {
            GameObject gameObject = Instantiate(elementPrefab.gameObject);
            gameObject.transform.SetParent(parent, false);
            StarterPackElementComponent component = gameObject.GetComponent<StarterPackElementComponent>();
            component.title.text = title;
            component.previewSkin.gameObject.GetComponent<Image>().preserveAspect = true;
            component.previewSkin.SpriteUid = spriteUid;

            if (count > 0) {
                component.count.gameObject.SetActive(true);
                component.count.text = "x" + count;
            } else {
                component.count.gameObject.SetActive(false);
            }

            if (needFrame) {
                component.RarityMask.enabled = true;
                component.RarityFrame.gameObject.SetActive(true);
                component.RarityFrame.SelectedSpriteIndex = (int)rarity;
                component.RarityMask.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                component.RarityFrame.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            } else {
                component.RarityMask.enabled = false;
                component.RarityFrame.gameObject.SetActive(false);
                component.RarityMask.transform.localScale = new Vector3(1f, 1f, 1f);
                component.RarityFrame.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            return gameObject;
        }

        public void OnClick() {
            OnPackClick(packEntity);
        }

        public void Clear() {
            methods.Clear();
        }

        public void Close() {
            MainScreenComponent.Instance.ShowMain();
        }
    }
}