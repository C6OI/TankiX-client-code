using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class DealsUIComponent : PurchaseItemComponent {
        [SerializeField] RectTransform bigRowsContainer;

        [SerializeField] RectTransform[] availableRows;

        [SerializeField] SpecialOfferContent specialOfferPrefab;

        [SerializeField] MarketItemSaleContent marketItemSalePrefab;

        [SerializeField] FirstPurchaseDiscountContent firstPurchaseDiscountPrefab;

        [SerializeField] GameObject bonusPrefab;

        [SerializeField] GameObject quantumPrefab;

        [SerializeField] public LeagueSpecialOfferComponent leagueSpecialOfferPrefab;

        [SerializeField] List<GiftPromo> promo;

        [SerializeField] ScrollRect scrollRect;

        [SerializeField] RectTransform scrollContent;

        [SerializeField] float autoScrollSpeed = 1f;

        [SerializeField] float pageWidth = 250f;

        [SerializeField] int pageCount;

        [SerializeField] int currentPage = 1;

        [SerializeField] bool interactWithScrollView;

        [SerializeField] bool scrollMode;

        [SerializeField] Button leftScrollButton;

        [SerializeField] Button rightScrollButton;

        [SerializeField] GameObject noDealsText;

        int contentRowsCount;

        List<long> marketItems;

        GameObject promoGameObject;

        bool secondBigRowFilled;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        void Start() {
            leftScrollButton.onClick.AddListener(ScrollLeft);
            rightScrollButton.onClick.AddListener(ScrollRight);
        }

        void Update() {
            UpdateScroll();

            if (Input.GetMouseButton(0)) {
                OnPointerDown();
            } else {
                OnPointerUp();
            }

            float axis = Input.GetAxis("Mouse ScrollWheel");

            if (axis > 0f) {
                ScrollLeft();
            } else if (axis < 0f) {
                ScrollRight();
            }
        }

        void OnEnable() {
            interactWithScrollView = false;
            marketItems = new List<long>();
        }

        public SpecialOfferContent AddSpecialOffer(Entity specialOffer, GameObject customPrefab = null) {
            contentRowsCount++;
            GameObject gameObject = PresentSpecilaOffer(specialOffer);

            if (gameObject != null) {
                gameObject.transform.parent = null;
                Destroy(gameObject);
                contentRowsCount--;
            }

            SpecialOfferContent component = AddItem(customPrefab ?? specialOfferPrefab.gameObject).GetComponent<SpecialOfferContent>();
            component.SetDataProvider(specialOffer);

            component.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                OnPackClick(specialOffer);
            });

            UpdateOrders();
            return component;
        }

        public void AddPromo(string key) {
            if (promoGameObject != null) {
                return;
            }

            GiftPromo giftPromo = promo.FirstOrDefault(x => x.Key == key);

            if (giftPromo != null) {
                GameObject prefab = giftPromo.Prefab;

                if (prefab != null) {
                    promoGameObject = Instantiate(prefab);
                    promoGameObject.transform.SetParent(transform, false);
                    contentRowsCount++;
                    UpdateOrders();
                }
            }
        }

        public void RemovePromo() {
            if (promoGameObject != null) {
                promoGameObject.transform.SetParent(null);
                promoGameObject.SetActive(false);
                Destroy(promoGameObject);
                promoGameObject = null;
                contentRowsCount--;
                UpdateOrders();
            }
        }

        public void RemoveSpecialOffer(Entity specialOffer) {
            GameObject gameObject = PresentSpecilaOffer(specialOffer);

            if (gameObject != null) {
                gameObject.transform.parent = null;
                Destroy(gameObject);
                contentRowsCount--;
            }

            UpdateOrders();
        }

        GameObject PresentSpecilaOffer(Entity specialOffer) {
            if (specialOffer != null) {
                SpecialOfferContent[] componentsInChildren = GetComponentsInChildren<SpecialOfferContent>();

                for (int i = 0; i < componentsInChildren.Length; i++) {
                    if (componentsInChildren[i].Entity == specialOffer) {
                        return componentsInChildren[i].gameObject;
                    }
                }
            }

            return null;
        }

        public void AddFirstPurchaseDiscount(double discount) {
            contentRowsCount++;
            FirstPurchaseDiscountContent component = AddItem(firstPurchaseDiscountPrefab.gameObject).GetComponent<FirstPurchaseDiscountContent>();
            component.Discount = discount;

            component.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                OnFirstPurchaseDiscountClick();
            });

            UpdateOrders();
        }

        public void AddBonus(Entity entity, Entity user) {
            contentRowsCount++;
            EnergyBonusContent component = AddItem(bonusPrefab).GetComponent<EnergyBonusContent>();
            component.Premium = user.HasComponent<PremiumAccountBoostComponent>();
            component.SetDataProvider(entity);

            component.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                OnBonusClick(entity);
            });

            UpdateOrders();
        }

        public void AddQuantum(Entity entity) {
            contentRowsCount++;
            QuantumShopContent component = AddItem(quantumPrefab).GetComponent<QuantumShopContent>();
            component.SetDataProvider(entity);

            component.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                OnQuantumClick(entity);
            });

            UpdateOrders();
        }

        public void AddMarketItem(Entity entity) {
            if (!marketItems.Contains(entity.Id)) {
                marketItems.Add(entity.Id);
                contentRowsCount++;
                MarketItemSaleContent component = AddItem(marketItemSalePrefab.gameObject).GetComponent<MarketItemSaleContent>();
                component.SetDataProvider(entity);

                component.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                    OnMarketItemClick(entity);
                });

                UpdateOrders();
            }
        }

        void UpdateOrders() {
            ContentWithOrder[] componentsInChildren = GetComponentsInChildren<ContentWithOrder>(true);
            Array.Sort(componentsInChildren, Compare);
            bool flag = false;
            secondBigRowFilled = false;
            int num = 0;
            availableRows[2].gameObject.SetActive(true);

            for (int i = 0; i < componentsInChildren.Length; i++) {
                bool flag2 = false;

                if (CheckUserItem(componentsInChildren[i])) {
                    continue;
                }

                if (num == 0 && i == 0) {
                    if (componentsInChildren[i].CanFillBigRow) {
                        flag = true;
                    } else {
                        num++;
                    }
                }

                if (flag && num == 1 && i == 1 && !componentsInChildren[i].CanFillSmallRow) {
                    availableRows[2].gameObject.SetActive(false);
                    flag2 = true;
                    secondBigRowFilled = true;
                }

                componentsInChildren[i].SetParent(availableRows[Mathf.Min(num, availableRows.Length - 1)]);

                if (flag2) {
                    num++;
                }

                num++;
            }

            if (!flag) {
                RectTransform parent = availableRows[availableRows.Length - 1];

                for (int j = 0; j < availableRows.Length - 1; j++) {
                    DealItemContent[] componentsInChildren2 = availableRows[j].GetComponentsInChildren<DealItemContent>(true);
                    DealItemContent[] array = componentsInChildren2;

                    foreach (DealItemContent dealItemContent in array) {
                        dealItemContent.SetParent(parent);
                        dealItemContent.transform.SetSiblingIndex(j - 1);
                    }
                }

                bigRowsContainer.gameObject.SetActive(false);
            } else {
                RectTransform rectTransform = availableRows[availableRows.Length - 1];
                DealItemContent[] componentsInChildren3 = rectTransform.GetComponentsInChildren<DealItemContent>(true);
                Array.Sort(componentsInChildren3, Compare);

                for (int l = 0; l < componentsInChildren3.Length; l++) {
                    componentsInChildren3[l].transform.SetSiblingIndex(l);
                }

                bigRowsContainer.gameObject.SetActive(true);
            }

            UpdateScroll();
            Layout();

            if (contentRowsCount == 0 && !noDealsText.activeSelf) {
                noDealsText.SetActive(true);
            } else if (contentRowsCount > 0 && noDealsText.activeSelf) {
                noDealsText.SetActive(false);
            }
        }

        int Compare(ContentWithOrder a, ContentWithOrder b) {
            if (a.Order > b.Order) {
                return 1;
            }

            if (a.Order < b.Order) {
                return -1;
            }

            return 0;
        }

        GameObject AddItem(GameObject prefab) {
            GameObject gameObject = Instantiate(prefab);
            gameObject.GetComponentInChildren<DealItemContent>().SetParent(transform);
            return gameObject;
        }

        void OnMarketItemClick(Entity entity) {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(entity);

            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(item, delegate {
                OnItemBuyCallback(item);
            }, item.XPrice);
        }

        void OnFirstPurchaseDiscountClick() {
            ShopTabManager.Instance.Show(3);
        }

        void OnItemBuyCallback(GarageItem item) {
            if (item.MarketItem.HasComponent<ContainerMarkerComponent>()) {
                EngineService.Engine.ScheduleEvent(new ShowGarageCategoryEvent {
                    Category = GarageCategory.CONTAINERS,
                    SelectedItem = item.MarketItem
                }, item.MarketItem);
            } else {
                UpdateOrders();
            }
        }

        void OnBonusClick(Entity entity) {
            EngineService.Engine.ScheduleEvent<TryUseBonusEvent>(entity);
        }

        void OnQuantumClick(Entity entity) {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(entity);
            item.MarketItem = entity;
            KeyValuePair<int, int> keyValuePair = entity.GetComponent<PackPriceComponent>().PackXPrice.First();
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(item, delegate { }, keyValuePair.Value, keyValuePair.Key);
        }

        bool CheckUserItem(object obj) {
            DealItemContent dealItemContent = obj as DealItemContent;

            if (dealItemContent == null) {
                return false;
            }

            if (dealItemContent.Entity != null && dealItemContent.Entity.HasComponent<GarageItemComponent>() && !dealItemContent.Entity.HasComponent<ContainerMarkerComponent>()) {
                GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(dealItemContent.Entity);

                if (item.UserItem != null) {
                    if (dealItemContent.gameObject.activeSelf) {
                        dealItemContent.gameObject.SetActive(false);
                        contentRowsCount--;
                    }

                    return true;
                }

                if (dealItemContent.Entity.HasComponent<XPriceItemComponent>()) {
                    XPriceItemComponent component = dealItemContent.Entity.GetComponent<XPriceItemComponent>();

                    if (!component.IsBuyable) {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Clear() {
            DealItemContent[] componentsInChildren = GetComponentsInChildren<DealItemContent>(true);
            DealItemContent[] array = componentsInChildren;

            foreach (DealItemContent dealItemContent in array) {
                Destroy(dealItemContent.gameObject);
            }

            methods.Clear();
            contentRowsCount = 0;
        }

        void UpdateScroll() {
            int num = !secondBigRowFilled ? 7 : 6;

            if (contentRowsCount > num) {
                if (!interactWithScrollView) {
                    pageCount = (contentRowsCount - (num - 1)) / 2 + 1;
                    Vector2 b = new(-(currentPage - 1) * pageWidth, 0f);
                    scrollContent.anchoredPosition = Vector2.Lerp(scrollContent.anchoredPosition, b, autoScrollSpeed * Time.deltaTime);
                }

                if (!scrollMode) {
                    ScrollOn();
                }
            } else if (scrollMode) {
                ScrollOff();
            }
        }

        void ScrollOn() {
            scrollRect.enabled = true;
            scrollMode = true;
            SetScrollButtons();
        }

        void ScrollOff() {
            scrollRect.enabled = false;
            scrollRect.horizontalScrollbar.gameObject.SetActive(false);
            scrollMode = false;
            SetScrollButtons();
            Layout();
        }

        void ScrollRight() {
            currentPage = Mathf.Min(pageCount, currentPage + 1);
            interactWithScrollView = false;
            SetScrollButtons();
        }

        void ScrollLeft() {
            currentPage = Mathf.Max(1, currentPage - 1);
            interactWithScrollView = false;
            SetScrollButtons();
        }

        public void OnPointerDown() {
            if (!interactWithScrollView && !PointerOnScrollButtons()) {
                interactWithScrollView = true;
            }
        }

        public void OnPointerUp() {
            if (scrollMode && interactWithScrollView) {
                interactWithScrollView = false;
                currentPage = Mathf.Min(Mathf.Max(0, Mathf.RoundToInt((0f - scrollContent.anchoredPosition.x) / pageWidth)) + 1, pageCount);
                SetScrollButtons();
            }
        }

        void SetScrollButtons() {
            leftScrollButton.GetComponent<CanvasGroup>().alpha = currentPage <= 1 || !scrollMode ? 0f : 1f;
            rightScrollButton.GetComponent<CanvasGroup>().alpha = currentPage >= pageCount || !scrollMode ? 0f : 1f;
        }

        void Layout() {
            if (!scrollMode) {
                Canvas.ForceUpdateCanvases();
                scrollContent.anchoredPosition = new Vector2((scrollRect.GetComponent<RectTransform>().rect.width - scrollContent.rect.width) / 2f, 0f);
            }
        }

        bool PointerOnScrollButtons() {
            PointerEventData pointerEventData = new(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> list = new();
            EventSystem.current.RaycastAll(pointerEventData, list);

            if (list.Count > 0) {
                foreach (RaycastResult item in list) {
                    if (item.gameObject == leftScrollButton.gameObject || item.gameObject == rightScrollButton) {
                        return true;
                    }
                }
            }

            return false;
        }

        [Serializable]
        public class GiftPromo {
            public string Key;

            public GameObject Prefab;
        }
    }
}