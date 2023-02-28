using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class Carousel : ECSBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, IEventSystemHandler {
        static bool axisBlockedAtCurrentTick;

        [SerializeField] RectTransform content;

        [SerializeField] float scrollThreshold;

        [SerializeField] GarageItemUI itemPrefab;

        [SerializeField] int selectedItem;

        [SerializeField] float scrollSpeed = 30f;

        [SerializeField] float fitDuration;

        [SerializeField] float inputThreshold;

        bool drag;

        float elapsedTime;

        float inputElapsedTime;

        readonly List<GarageItemUI> items = new();

        int itemsCount;

        float lastScrollTime;

        public UnityAction<GarageItemUI> onItemSelected;

        int prevSelectedItem = -1;

        float scrollDelta;

        float startPosition;

        float targetPosition;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public GarageItemUI Selected => items[selectedItem];

        public bool IsAnySelected => selectedItem >= 0;

        void LateUpdate() {
            if (items.Count == 0 || selectedItem < 0) {
                elapsedTime = 0f;
                return;
            }

            if (selectedItem != prevSelectedItem) {
                if (prevSelectedItem >= 0) {
                    items[prevSelectedItem].Deselect();
                }

                items[selectedItem].Select();
                onItemSelected(items[selectedItem]);
                prevSelectedItem = selectedItem;
            }

            if (drag) {
                elapsedTime = 0f;
                return;
            }

            float axis = Input.GetAxis("Horizontal");
            CheckForTutorialEvent checkForTutorialEvent = new();
            ScheduleEvent(checkForTutorialEvent, EngineService.EntityStub);

            if (!axisBlockedAtCurrentTick && axis != 0f && inputElapsedTime >= inputThreshold && !checkForTutorialEvent.TutorialIsActive) {
                if (axis > 0f) {
                    selectedItem = Mathf.Min(selectedItem + 1, itemsCount - 1);
                } else if (axis < 0f) {
                    selectedItem = Mathf.Max(selectedItem - 1, 0);
                }

                inputElapsedTime = 0f;
            } else {
                inputElapsedTime += Time.deltaTime;
            }

            axisBlockedAtCurrentTick = false;
            targetPosition = 0f - items[selectedItem].RectTransform.anchoredPosition.x;

            if (!Mathf.Approximately(content.anchoredPosition.x, targetPosition)) {
                Vector2 anchoredPosition = content.anchoredPosition;

                if (elapsedTime == 0f) {
                    startPosition = anchoredPosition.x;
                }

                anchoredPosition.x = Mathf.MoveTowards(anchoredPosition.x, targetPosition, Time.deltaTime * Mathf.Abs(startPosition - targetPosition) / fitDuration);
                content.anchoredPosition = anchoredPosition;
                elapsedTime += Time.deltaTime;
            } else {
                elapsedTime = 0f;
            }
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!TutorialCanvas.Instance.IsShow) {
                drag = true;
            }
        }

        public void OnDrag(PointerEventData eventData) {
            if (!TutorialCanvas.Instance.IsShow) {
                Vector2 anchoredPosition = content.anchoredPosition;
                anchoredPosition.x += eventData.delta.x;
                content.anchoredPosition = anchoredPosition;
                int centerItem = GetCenterItem();

                if (centerItem >= 0) {
                    selectedItem = centerItem;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!TutorialCanvas.Instance.IsShow) {
                drag = false;
                int centerItem = GetCenterItem();

                if (centerItem >= 0) {
                    selectedItem = centerItem;
                }
            }
        }

        public void OnScroll(PointerEventData eventData) {
            if (!TutorialCanvas.Instance.IsShow && lastScrollTime + scrollThreshold < Time.time) {
                scrollDelta = eventData.scrollDelta.y;
                lastScrollTime = Time.time;

                if (scrollDelta < 0f) {
                    selectedItem = Mathf.Min(selectedItem + 1, itemsCount - 1);
                } else {
                    selectedItem = Mathf.Max(selectedItem - 1, 0);
                }
            }
        }

        public static void BlockAxisAtCurrentTick() {
            axisBlockedAtCurrentTick = true;
        }

        public void AddItems<T>(List<T> newItems) where T : GarageItem {
            selectedItem = -1;
            prevSelectedItem = -1;

            if (items.Count > newItems.Count) {
                for (int i = newItems.Count; i < items.Count; i++) {
                    items[i].gameObject.SetActive(false);
                }
            } else if (items.Count < newItems.Count) {
                for (int j = items.Count; j < newItems.Count; j++) {
                    AddItem();
                }
            }

            for (int k = 0; k < newItems.Count; k++) {
                items[k].gameObject.SetActive(true);
                items[k].Init(newItems[k], this);
            }

            itemsCount = newItems.Count;
        }

        public void RemoveItem(long marketItemId) {
            selectedItem = -1;
            prevSelectedItem = -1;
            GarageItemUI garageItemUI = null;

            foreach (GarageItemUI item in items) {
                if (item.Item.MarketItem.Id == marketItemId) {
                    garageItemUI = item;
                }
            }

            if (garageItemUI != null) {
                items.Remove(garageItemUI);
                Destroy(garageItemUI.gameObject);
            }

            itemsCount = items.Count;
        }

        void AddItem() {
            GarageItemUI garageItemUI = Instantiate(itemPrefab);
            garageItemUI.transform.SetParent(content, false);
            items.Add(garageItemUI);
        }

        public void Select(GarageItem item, bool immediately = false) {
            if (selectedItem >= 0 && items[selectedItem].Item == item) {
                return;
            }

            for (int i = 0; i < itemsCount; i++) {
                GarageItemUI garageItemUI = items[i];

                if (garageItemUI.Item == item) {
                    selectedItem = i;

                    if (immediately) {
                        targetPosition = 0f - items[selectedItem].RectTransform.anchoredPosition.x;
                        Vector2 anchoredPosition = content.anchoredPosition;
                        anchoredPosition.x = targetPosition;
                        content.anchoredPosition = anchoredPosition;
                    }
                }
            }
        }

        int GetCenterItem() {
            float x = content.anchoredPosition.x;
            int result = -1;
            float num = float.PositiveInfinity;

            for (int i = 0; i < itemsCount; i++) {
                GarageItemUI garageItemUI = items[i];
                float num2 = Mathf.Abs(garageItemUI.RectTransform.anchoredPosition.x + x);

                if (num2 < num) {
                    result = i;
                    num = num2;
                }
            }

            return result;
        }
    }
}