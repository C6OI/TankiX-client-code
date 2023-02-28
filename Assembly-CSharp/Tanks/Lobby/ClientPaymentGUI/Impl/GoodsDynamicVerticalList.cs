using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientControls.API.List;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class GoodsDynamicVerticalList : MonoBehaviour {
        [SerializeField]
        public enum GoodsType {
            XCrystals = 0,
            SpecialOffer = 1
        }

        const int commentSize = 200;

        [SerializeField] GameObject commentPrefab;

        [SerializeField] RectTransform item;

        [SerializeField] List<GoodsContentAdapter> Adapters;

        [SerializeField] int spacing;

        [SerializeField] RectTransform viewport;

        readonly List<Text> Comments = new();

        readonly Dictionary<GoodsType, List<ListItem>> generatedItems = new();

        RectTransform rectTransform;

        int visibleItemsCount;

        float end => rectTransform.anchoredPosition.y + viewport.rect.height;

        float start => rectTransform.anchoredPosition.y;

        void Awake() {
            Adapters.Find(x => x.Type == GoodsType.XCrystals).DataProvider = GetComponent<XCrystalsDataProvider>();
            Adapters.Find(x => x.Type == GoodsType.SpecialOffer).DataProvider = GetComponent<SpecialOfferDataProvider>();
            rectTransform = GetComponent<RectTransform>();
        }

        void Update() {
            Layout();
        }

        void OnEnable() {
            foreach (GoodsContentAdapter adapter in Adapters) {
                adapter.DataProvider.DataChanged += UpdateBounds;
            }
        }

        void OnDisable() {
            foreach (GoodsContentAdapter adapter in Adapters) {
                adapter.DataProvider.DataChanged -= UpdateBounds;
            }

            rectTransform.anchoredPosition = new Vector2(0f, 0f);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
        }

        void UpdateBounds(ListDataProvider provider) {
            UpdateSize();
        }

        void UpdateSize() {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, viewport.rect.width - 1f);
            int height = 0;

            Adapters.ForEach(delegate(GoodsContentAdapter x) {
                height += (x.Content.Height + spacing) * x.DataProvider.Data.Count;
                height += x.DataProvider.CommentCount * 200;
            });

            height -= spacing;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            if (height < rectTransform.anchoredPosition.y) {
                rectTransform.anchoredPosition = new Vector2(0f, Math.Max(0f, height - viewport.rect.height));
            }
        }

        void OnItemSelect(ListItem listItem) {
            foreach (GoodsContentAdapter adapter in Adapters) {
                Entity entity = adapter.DataProvider as Entity;

                if (entity != null && entity.HasComponent<SelectedListItemComponent>()) {
                    entity.RemoveComponent<SelectedListItemComponent>();
                }
            }
        }

        void Layout() {
            int num = 0;
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            int num5 = 0;

            foreach (GoodsContentAdapter adapter in Adapters) {
                if (!generatedItems.ContainsKey(adapter.Type)) {
                    generatedItems[adapter.Type] = new List<ListItem>();
                }

                num4 = 0;

                foreach (object datum in adapter.DataProvider.Data) {
                    if (num3 - start > end) {
                        break;
                    }

                    num2 = num3 + adapter.Content.Height;

                    if (num2 >= start) {
                        if (num4 == generatedItems[adapter.Type].Count) {
                            RectTransform rectTransform = Instantiate(item);
                            ListItem component = rectTransform.GetComponent<ListItem>();
                            component.SetContent(Instantiate(adapter.Content.Prefab));
                            generatedItems[adapter.Type].Add(component);
                            rectTransform.SetParent(this.rectTransform, false);
                            Vector2 anchorMax = rectTransform.anchorMin = new Vector2(0f, 1f);
                            rectTransform.anchorMax = anchorMax;
                            rectTransform.pivot = new Vector2(0f, 1f);
                        } else {
                            generatedItems[adapter.Type][num4].gameObject.SetActive(true);
                        }

                        RectTransform component2 = generatedItems[adapter.Type][num4].GetComponent<RectTransform>();
                        component2.anchoredPosition = new Vector2(0f, 0f - num3);
                        component2.sizeDelta = new Vector2(viewport.rect.width, adapter.Content.Height);
                        generatedItems[adapter.Type][num4].Data = datum;
                        num4++;

                        if (adapter.DataProvider.HasComment(datum)) {
                            if (num5 == Comments.Count) {
                                GameObject gameObject = Instantiate(commentPrefab);
                                Text component3 = gameObject.GetComponent<Text>();
                                component3.transform.SetParent(rectTransform, false);
                                Comments.Add(component3);
                            } else {
                                Comments[num5].gameObject.SetActive(true);
                            }

                            Comments[num5].text = adapter.DataProvider.GetComment(datum);
                            RectTransform component4 = Comments[num5].GetComponent<RectTransform>();
                            component4.anchoredPosition = new Vector2(0f, 0f - num2);
                            component4.sizeDelta = new Vector2(viewport.rect.width, 200f);
                            num5++;
                            num2 += 200f;
                        }
                    }

                    num3 = num2 + spacing;
                }

                for (int i = num4; i < generatedItems[adapter.Type].Count; i++) {
                    generatedItems[adapter.Type][i].gameObject.SetActive(false);
                }

                for (int j = num5; j < Comments.Count; j++) {
                    Comments[j].gameObject.SetActive(false);
                }
            }
        }

        [Serializable]
        public class ItemContent {
            public RectTransform Prefab;

            public int Height;
        }

        [Serializable]
        public class ContentAdapter {
            public ItemContent Content;

            public CommentedListDataProvider DataProvider;
        }

        [Serializable]
        public class GoodsContentAdapter : ContentAdapter {
            public GoodsType Type;
        }
    }
}