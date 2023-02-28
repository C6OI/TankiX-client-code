using System;
using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API.List;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class DynamicVerticalList : MonoBehaviour {
        [SerializeField] RectTransform item;

        [SerializeField] RectTransform itemContent;

        [SerializeField] int itemHeight;

        [SerializeField] int spacing;

        [SerializeField] RectTransform viewport;

        ListDataProvider dataProvider;

        readonly List<ListItem> generatedItems = new();

        RectTransform rectTransform;

        ListItem selected;

        int visibleItemsCount;

        void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        void Update() {
            CalculateVisibleItems();
            Layout();
        }

        void OnEnable() {
            dataProvider = GetComponent<ListDataProvider>();

            if (dataProvider == null) {
                dataProvider = gameObject.AddComponent<DefaultListDataProvider>();
            }

            dataProvider.DataChanged += UpdateBounds;
            UpdateBounds(dataProvider);
        }

        void OnDisable() {
            dataProvider.DataChanged -= UpdateBounds;
            rectTransform.anchoredPosition = new Vector2(0f, 0f);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
        }

        void UpdateBounds(ListDataProvider provider) {
            UpdateSize();

            if (provider.Selected == null) {
                return;
            }

            ScrollToSelection();
            Layout();

            foreach (ListItem generatedItem in generatedItems) {
                if (generatedItem.Data == provider.Selected) {
                    generatedItem.Select(false);
                    break;
                }
            }
        }

        void UpdateSize() {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, viewport.rect.width - 1f);
            int num = dataProvider.Data.Count * itemHeight + (dataProvider.Data.Count - 1) * spacing;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num);

            if (num < rectTransform.anchoredPosition.y) {
                rectTransform.anchoredPosition = new Vector2(0f, Math.Min(0f, num - viewport.rect.height));
            }
        }

        void OnItemSelect(ListItem listItem) {
            if (selected != null) {
                selected.PlayDeselectionAnimation();
            }

            selected = listItem;
        }

        void CalculateVisibleItems() {
            visibleItemsCount = Math.Min((int)(viewport.rect.height / (itemHeight + spacing) + 2f), dataProvider.Data.Count);

            if (visibleItemsCount > generatedItems.Count) {
                for (int i = generatedItems.Count; i < visibleItemsCount; i++) {
                    RectTransform rectTransform = Instantiate(item);
                    ListItem component = rectTransform.GetComponent<ListItem>();
                    component.SetContent(Instantiate(itemContent));
                    generatedItems.Add(component);
                    rectTransform.SetParent(this.rectTransform, false);
                    Vector2 anchorMax = rectTransform.anchorMin = new Vector2(0f, 1f);
                    rectTransform.anchorMax = anchorMax;
                    rectTransform.pivot = new Vector2(0f, 1f);
                }
            }
        }

        public void ScrollToSelection() {
            float num = rectTransform.anchoredPosition.y;

            if (num < 0f) {
                rectTransform.anchoredPosition = default;
                num = 0f;
            }

            if (dataProvider.Selected == null) {
                return;
            }

            int num2 = dataProvider.Data.IndexOf(dataProvider.Selected);

            if (num2 >= 0) {
                int num3 = num2 * (itemHeight + spacing);
                float num4 = num3 - num;

                if (num4 + itemHeight > viewport.rect.height || num4 < 0f) {
                    float y = Math.Min(num3, rectTransform.rect.height - viewport.rect.height);
                    rectTransform.anchoredPosition = new Vector2(0f, y);
                }
            }
        }

        void Layout() {
            int num = (int)(rectTransform.anchoredPosition.y / (itemHeight + spacing));

            if (num < 0) {
                rectTransform.anchoredPosition = default;
                num = 0;
            }

            for (int i = 0; i < visibleItemsCount && num + i < dataProvider.Data.Count; i++) {
                ListItem listItem = generatedItems[i];

                if (!listItem.gameObject.activeSelf) {
                    listItem.gameObject.SetActive(true);
                }

                RectTransform component = listItem.GetComponent<RectTransform>();
                component.anchoredPosition = new Vector2(0f, -(num + i) * (itemHeight + spacing));
                component.sizeDelta = new Vector2(viewport.rect.width, itemHeight);
                listItem.Data = dataProvider.Data[num + i];

                if (listItem.Data == dataProvider.Selected) {
                    listItem.PlaySelectionAnimation();
                } else {
                    listItem.PlayDeselectionAnimation();
                }
            }

            for (int j = visibleItemsCount; j < generatedItems.Count; j++) {
                ListItem listItem2 = generatedItems[j];

                if (listItem2.gameObject.activeSelf) {
                    listItem2.gameObject.SetActive(false);
                }
            }
        }
    }
}