using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = UnityEngine.Event;

namespace Tanks.Lobby.ClientControls.API {
    public class SimpleHorizontalListComponent : MonoBehaviour, IScrollHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, Component, IEventSystemHandler {
        [SerializeField] RectTransform horizontalLayoutGroup;

        [SerializeField] RectTransform leftButtonPlace;

        [SerializeField] RectTransform rightButtonPlace;

        [SerializeField] RectTransform content;

        [SerializeField] RectTransform scrollRect;

        [SerializeField] ListItem itemPrefab;

        [SerializeField] EntityBehaviour itemContentPrefab;

        [SerializeField] RectTransform navigationButtonPrefab;

        [SerializeField] float navigationButtonsScrollTime = 0.3f;

        bool animating;

        int calculatedItemWidth;

        Canvas canvas;

        bool checkedSorter;

        float dragDirection;

        bool draging;

        readonly ItemsMap items = new();

        RectTransform leftButton;

        float navigationButtonsScrollVelocity;

        bool needDestroyNavigationButton;

        bool noScroll;

        float position;

        int previousScrollRectWidth;

        RectTransform rightButton;

        ListItem selectedItem;

        SimpleHorizontalListItemsSorter sorter;

        float spaceBetweenElements;

        float spaceBetweenNavigationButtons;

        int targetItemIndex;

        float velocity;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        RectTransform RectTransform => (RectTransform)transform;

        public SimpleHorizontalListItemsSorter Sorter {
            get {
                if (sorter == null && !checkedSorter) {
                    sorter = GetComponent<SimpleHorizontalListItemsSorter>();
                    checkedSorter = true;
                }

                return sorter;
            }
        }

        public bool IsKeyboardNavigationAllowed { get; set; } = true;

        public int Count => items.Count;

        void Awake() {
            spaceBetweenNavigationButtons = horizontalLayoutGroup.GetComponent<SimpleHorizontalLayoutGroup>().spacing;
            previousScrollRectWidth = (int)scrollRect.rect.width;
            float width = navigationButtonPrefab.rect.width;
            leftButtonPlace.GetComponent<SimpleLayoutElement>().maxWidth = width;
            rightButtonPlace.GetComponent<SimpleLayoutElement>().maxWidth = width;
        }

        void Start() {
            Layout();
        }

        void Update() {
            if (draging) {
                return;
            }

            int itemIndex = CorrectIndex(targetItemIndex);
            float num = ItemIndexToPosition(itemIndex);

            if (position == num) {
                return;
            }

            if (!animating) {
                animating = true;
                content.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }

            if (position < num) {
                position += velocity * Time.deltaTime;

                if (position > num) {
                    position = num;
                    ClearHighlight();
                }

                ApplyPosition();
            } else {
                position -= velocity * Time.deltaTime;

                if (position < num) {
                    position = num;
                    ClearHighlight();
                }

                ApplyPosition();
            }
        }

        void OnGUI() {
            if (needDestroyNavigationButton) {
                needDestroyNavigationButton = false;
                DestroyNavigationButtons();
            }

            if (!previousScrollRectWidth.Equals((int)scrollRect.rect.width)) {
                previousScrollRectWidth = (int)scrollRect.rect.width;
                Layout();
                SetPositionToTarget();
            }

            if (IsKeyboardNavigationAllowed && !noScroll && Event.current.type == EventType.KeyDown) {
                float horizontal = InputMapping.Horizontal;
                int num = -1;

                if (horizontal > 0f) {
                    num = selectedItem.transform.GetSiblingIndex() + 1;
                } else if (horizontal < 0f) {
                    num = selectedItem.transform.GetSiblingIndex() - 1;
                }

                if (num >= 0 && num < content.childCount) {
                    Transform child = content.GetChild(num);
                    child.GetComponent<ListItem>().Select();
                    MoveToItemAnimated(child.GetComponent<ListItem>().Data);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!noScroll) {
                draging = true;
                dragDirection = 0f;
            }
        }

        public void OnDrag(PointerEventData eventData) {
            if (!noScroll) {
                if (canvas == null) {
                    BaseElementCanvasScaler componentInParent = GetComponentInParent<BaseElementCanvasScaler>();
                    canvas = componentInParent.GetComponent<Canvas>();
                }

                float num = eventData.delta.x / canvas.scaleFactor;
                position += num;
                dragDirection += num;
                ApplyPosition();
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!noScroll) {
                draging = false;
                ToNearestItem();
                ClearHighlight();
            }
        }

        public void OnScroll(PointerEventData eventData) {
            if (!noScroll && !draging) {
                if (eventData.scrollDelta.y > 0f) {
                    OnLeftButtonClick();
                } else {
                    OnRightButtonClick();
                }
            }
        }

        public ICollection<Entity> GetItems() {
            return items.Select(item => (Entity)item.Data).ToList();
        }

        public GameObject GetItem(Entity entity) => items[entity].gameObject;

        public void AddItem(Entity entity) {
            EntityBehaviour entityBehaviour = Instantiate(itemContentPrefab);
            ListItem listItem = Instantiate(itemPrefab);
            listItem.SetContent((RectTransform)entityBehaviour.transform);
            listItem.transform.SetParent(content, false);
            listItem.Data = entity;
            items.Add(listItem);
            entityBehaviour.BuildEntity(entity);

            if (Sorter != null) {
                Sorter.Sort(items);
                int num = 0;

                foreach (ListItem item in items) {
                    item.transform.SetSiblingIndex(num++);
                }
            } else {
                listItem.transform.SetAsLastSibling();
            }

            Layout();
        }

        public void RemoveItem(Entity entity) {
            if (!items.Contains(entity)) {
                throw new ItemNotExistsException(entity);
            }

            ListItem listItem = items[entity];

            if (selectedItem == listItem) {
                selectedItem = null;
            }

            items.Remove(entity);
            EntityBehaviour.CleanUp(listItem.gameObject);
            DestroyImmediate(listItem.gameObject);
            Layout();
        }

        public bool Contains(Entity entity) => items.Contains(entity);

        public void Select(Entity entity) {
            if (!items.Contains(entity)) {
                throw new ItemNotExistsException(entity);
            }

            OnItemSelect(items[entity]);
            items[entity].Select();
        }

        public void MoveToItem(Entity entity) {
            targetItemIndex = items[entity].transform.GetSiblingIndex();
            velocity = navigationButtonsScrollVelocity;
            SetPositionToTarget();
            ClearHighlight();
        }

        public void MoveToItem(GameObject obj) {
            Entity entity = obj.GetComponentInParent<EntityBehaviour>().Entity;

            if (items.Contains(entity)) {
                MoveToItem(entity);
            }
        }

        public void MoveToItemAnimated(object entity) {
            targetItemIndex = items[entity].transform.GetSiblingIndex();
            velocity = navigationButtonsScrollVelocity;
        }

        public void ClearItems(bool immediate = false) {
            foreach (ListItem item in items) {
                EntityBehaviour.CleanUp(item.gameObject);

                if (immediate) {
                    DestroyImmediate(item.gameObject);
                } else {
                    Destroy(item.gameObject);
                }
            }

            items.Clear();
            selectedItem = null;
            position = 0f;
            targetItemIndex = 0;
            velocity = 0f;
            animating = false;
            ApplyPosition();
        }

        public int GetIndex(Entity entity) => items[entity].transform.GetSiblingIndex();

        public void SetIndex(Entity entity, int index) {
            items[entity].transform.SetSiblingIndex(index);
        }

        void Layout() {
            LayoutElement component = itemPrefab.GetComponent<LayoutElement>();
            int count = items.Count;
            calculatedItemWidth = CalculateItemWidth();
            float minWidth = component.minWidth;
            spaceBetweenElements = content.GetComponent<HorizontalLayoutGroup>().spacing;
            int num = (int)(minWidth * count + spaceBetweenElements * (count - 1));

            if (num > RectTransform.rect.width) {
                LayoutWithNavigationButtons();
            } else {
                LayoutWithoutNavigationButtons();

                if (component.preferredWidth > 0f) {
                    calculatedItemWidth = Mathf.Min(calculatedItemWidth, (int)component.preferredWidth);
                }
            }

            ExtendItemsWidth(calculatedItemWidth);
            navigationButtonsScrollVelocity = (calculatedItemWidth + spaceBetweenElements) / navigationButtonsScrollTime;
        }

        void LayoutWithNavigationButtons() {
            noScroll = false;
            horizontalLayoutGroup.GetComponent<SimpleHorizontalLayoutGroup>().spacing = spaceBetweenNavigationButtons;
            leftButtonPlace.GetComponent<SimpleLayoutElement>().flexibleWidth = 1f;
            rightButtonPlace.GetComponent<SimpleLayoutElement>().flexibleWidth = 1f;
            CreateNavigationButtons();
        }

        void CreateNavigationButtons() {
            if (leftButton == null) {
                leftButton = Instantiate(navigationButtonPrefab);
                InitNavigationButton(leftButton, leftButtonPlace.transform, -1, OnLeftButtonClick);
            }

            if (rightButton == null) {
                rightButton = Instantiate(navigationButtonPrefab);
                InitNavigationButton(rightButton, rightButtonPlace.transform, 1, OnRightButtonClick);
            }
        }

        void InitNavigationButton(RectTransform button, Transform parent, int scale, UnityAction clickHandler) {
            button.GetComponent<Button>().onClick.AddListener(clickHandler);
            button.SetParent(parent, false);
            button.localScale = new Vector3(scale, 1f, 1f);
            button.pivot = new Vector2(0.5f - scale * 0.5f, 0.5f);
            button.anchorMin = new Vector2(0f, 0.5f);
            button.anchorMax = new Vector2(0f, 0.5f);
        }

        void OnLeftButtonClick() {
            if (targetItemIndex > 0) {
                if (targetItemIndex > GetMaxTargetIndex()) {
                    targetItemIndex = GetMaxTargetIndex();
                }

                targetItemIndex--;
                velocity = navigationButtonsScrollVelocity;
            }
        }

        void OnRightButtonClick() {
            if (targetItemIndex < GetMaxTargetIndex()) {
                targetItemIndex++;
                velocity = navigationButtonsScrollVelocity;
            }
        }

        float GetMinPosition() => Mathf.Min(0f, scrollRect.rect.width - CalculateItemsWidth(items.Count, calculatedItemWidth));

        int GetMaxTargetIndex() {
            float minPosition = GetMinPosition();
            return PositionToItemIndex(minPosition);
        }

        void LayoutWithoutNavigationButtons() {
            noScroll = true;
            needDestroyNavigationButton = true;
            ClearHighlight();
            horizontalLayoutGroup.GetComponent<SimpleHorizontalLayoutGroup>().spacing = 0f;
            leftButtonPlace.GetComponent<SimpleLayoutElement>().flexibleWidth = 0f;
            rightButtonPlace.GetComponent<SimpleLayoutElement>().flexibleWidth = 0f;
            targetItemIndex = 0;
            position = 0f;
            ApplyPosition();
        }

        int CalculateItemWidth() {
            float minWidth = itemPrefab.GetComponent<LayoutElement>().minWidth;
            int i;

            for (i = 1; CalculateItemsWidth(i + 1, minWidth) < scrollRect.rect.width; i++) { }

            i = Mathf.Min(i, items.Count);
            int num = CalculateItemsWidth(i, minWidth);
            int num2 = (int)((scrollRect.rect.width - num) / i);
            return (int)(minWidth + num2);
        }

        void ExtendItemsWidth(int newItemWidth) {
            foreach (ListItem item in items) {
                item.GetComponent<LayoutElement>().minWidth = newItemWidth;
            }
        }

        void SetPositionToTarget() {
            position = ItemIndexToPosition(CorrectIndex(targetItemIndex));
            ApplyPosition();
        }

        float ItemIndexToPosition(int itemIndex) => -itemIndex * (calculatedItemWidth + spaceBetweenElements);

        void DestroyNavigationButtons() {
            if (leftButton != null) {
                Destroy(leftButton.gameObject);
                leftButton = null;
            }

            if (rightButton != null) {
                Destroy(rightButton.gameObject);
                rightButton = null;
            }
        }

        int CalculateItemsWidth(int count, float itemWidth) => (int)(itemWidth * count + spaceBetweenElements * (count - 1));

        void ClearHighlight() {
            animating = false;
            content.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        void OnBaseElementSizeChanged() {
            Layout();
            ToNearestItem();
        }

        void ToNearestItem() {
            float num = position % (calculatedItemWidth + spaceBetweenElements);
            targetItemIndex = !(dragDirection < 0f) ? PositionToItemIndex(position - num) : PositionToItemIndex(position - (calculatedItemWidth + spaceBetweenElements + num));
            velocity = navigationButtonsScrollVelocity;
            ApplyPosition();
        }

        void ApplyPosition() {
            position = ClampPosition(position);
            content.anchoredPosition = new Vector2(position, 0f);
            UpdateNavigationButtonsVisibility();
        }

        float ClampPosition(float pos) {
            if (pos >= 0f) {
                return 0f;
            }

            float minPosition = GetMinPosition();

            if (pos < minPosition) {
                return minPosition;
            }

            return pos;
        }

        void OnItemSelect(ListItem item) {
            if (selectedItem != null) {
                selectedItem.PlayDeselectionAnimation();
            }

            selectedItem = item;
            EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>((Entity)selectedItem.Data);
        }

        void UpdateNavigationButtonsVisibility() {
            if (leftButton != null) {
                leftButton.gameObject.SetActive(position < 0f);
            }

            if (rightButton != null) {
                float num = scrollRect.rect.width - CalculateItemsWidth(items.Count, calculatedItemWidth);
                rightButton.gameObject.SetActive(position > num);
            }
        }

        int CorrectIndex(int index) {
            if (index < 0) {
                return 0;
            }

            int maxTargetIndex = GetMaxTargetIndex();

            if (index > maxTargetIndex) {
                return maxTargetIndex;
            }

            return index;
        }

        int PositionToItemIndex(float pos) => Mathf.RoundToInt((0f - pos) / (calculatedItemWidth + spaceBetweenElements));
    }
}