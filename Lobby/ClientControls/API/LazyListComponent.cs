using System.Collections;
using log4net;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = UnityEngine.Event;

namespace Lobby.ClientControls.API {
    public class LazyListComponent : UIBehaviour, ILazyList, IEventSystemHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler, IScrollHandler, Component {
        const float EPSILON = 0.01f;

        [SerializeField] ListItem itemPrefab;

        [SerializeField] EntityBehaviour entityBehaviour;

        [SerializeField] float itemMinSize = 100f;

        [SerializeField] float spacing;

        [SerializeField] bool vertical;

        [SerializeField] bool noScroll;

        [SerializeField] float itemScrollTime = 0.2f;

        float allContentSize;

        Canvas canvas;

        bool dragDirectionPositive;

        bool dragging;

        bool forceNextUpdate = true;

        bool inSelectMode;

        int itemsCount;

        float itemSize;

        int itemsPerPage;

        ILog log;

        float pageSize;

        float position;

        float prevPageSize;

        bool quiting;

        RectTransform rectTransform;

        IndexRange screenRange;

        Entity selectedEntity;

        int selectedItemIndex;

        int targetItemIndex;

        float targetPosition;

        IndexRange visibleItemsRange;

        public bool AtLimitLow { get; private set; }

        public bool AtLimitHigh { get; private set; }

        protected override void Awake() {
            log = LoggerProvider.GetLogger(this);
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        void Update() {
            if (!AtTarget() || IsSizeChanged() || dragging || forceNextUpdate) {
                forceNextUpdate = false;
                Layout();
            }
        }

        protected override void OnDisable() {
            if (!quiting) {
                ClearItems();
                forceNextUpdate = true;
            }
        }

        void OnGUI() {
            if (Event.current.type == EventType.KeyDown && !inSelectMode) {
                float horizontal = InputMapping.Horizontal;

                if (horizontal > 0f) {
                    StartCoroutine(DelaySelection(1));
                } else if (horizontal < 0f) {
                    StartCoroutine(DelaySelection(-1));
                }
            }
        }

        void OnApplicationQuit() => quiting = true;

        public void OnBeginDrag(PointerEventData eventData) {
            if (!noScroll) {
                targetPosition = position;
                dragging = true;
            }
        }

        public void OnDrag(PointerEventData eventData) {
            if (!noScroll) {
                float num = eventData.delta[GetAxis()] / canvas.scaleFactor;
                position -= num;
                dragDirectionPositive = num >= 0f;
                targetPosition = position;
                ClampPosition();
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!noScroll) {
                dragging = false;
                targetItemIndex = PositionToItemIndex(position);

                if (!dragDirectionPositive) {
                    targetItemIndex++;
                }

                targetPosition = GetItemStartPosition(targetItemIndex);
                ClampPosition();
            }
        }

        public int ItemsCount {
            get => itemsCount;
            set {
                if (itemsCount != value) {
                    itemsCount = value;
                    Layout();
                }
            }
        }

        public IndexRange VisibleItemsRange => visibleItemsRange;

        public RectTransform GetItemContent(int itemIndex) {
            RectTransform item = GetItem(itemIndex);

            if (item != null) {
                return item.GetComponent<ListItem>().GetContent();
            }

            return null;
        }

        public void SetItemContent(int itemIndex, RectTransform content) {
            log.DebugFormat("SetItemContent itemIndex={0}", itemIndex);
            RectTransform item = GetItem(itemIndex);

            if (item != null) {
                item.GetComponent<ListItem>().SetContent(content);
            }
        }

        public void Scroll(int deltaItems) {
            targetItemIndex += deltaItems;
            ClampTargetItemIndex();
            targetPosition = GetItemStartPosition(targetItemIndex);
            ClampPosition();
        }

        public void ClearItems() {
            UnselectItemContent();
            UpdateVisibility(default, CalculateScreenRange(), false);
            screenRange = default;
            itemsCount = 0;
            position = 0f;
            targetPosition = 0f;
            targetItemIndex = 0;
            dragging = false;
            selectedItemIndex = 0;
        }

        public void OnScroll(PointerEventData eventData) {
            if (!noScroll) {
                if (eventData.scrollDelta.y > 0f) {
                    Scroll(-1);
                } else {
                    Scroll(1);
                }
            }
        }

        public void UpdateSelection(int itemIndex) {
            if (itemIndex == selectedItemIndex) {
                SelectItemContent();
            }
        }

        public void OnRemoveItemContent(int itemIndex) {
            log.DebugFormat("OnRemoveItemContent itemIndex={0}", itemIndex);

            if (itemIndex == selectedItemIndex) {
                UnselectItemContent();
            }
        }

        IEnumerator DelaySelection(int dir) {
            if (inSelectMode) {
                yield break;
            }

            int newIndex = selectedItemIndex + dir;

            if (newIndex >= itemsCount || newIndex < 0) {
                yield break;
            }

            inSelectMode = true;
            int index = ItemIndexToChildIndex(newIndex);
            float waitTime2 = 0f;

            if (index == -1) {
                Scroll(dir);

                while (index == -1) {
                    yield return new WaitForEndOfFrame();

                    index = ItemIndexToChildIndex(newIndex);
                    waitTime2 += Time.deltaTime;

                    if (waitTime2 > 1f) {
                        inSelectMode = false;
                        yield break;
                    }
                }
            }

            waitTime2 = 0f;

            while (GetItemContent(newIndex) == null) {
                yield return new WaitForEndOfFrame();

                waitTime2 += Time.deltaTime;

                if (waitTime2 > 1f) {
                    inSelectMode = false;
                    yield break;
                }
            }

            GetItem(newIndex).GetComponent<ListItem>().Select();
            inSelectMode = false;
        }

        void Layout() {
            bool sizeChanged = IsSizeChanged();
            UpdatePageSizes();
            UpdatePagePosition(sizeChanged);
            UpdateVisibility(CalculateVisibleItemsRange(), CalculateScreenRange(), true);
            UpdateItemsPositionsAndSizes();
            SendScrollLimitEvent();
        }

        bool IsSizeChanged() => GetSize() != prevPageSize;

        void UpdatePageSizes() {
            pageSize = GetSize();
            itemsPerPage = GetItemsPerPage(itemMinSize, spacing, pageSize);
            itemSize = GetItemSize(pageSize, itemsPerPage, spacing);
            allContentSize = GetAllContentSize(itemsCount, itemSize, spacing);
            prevPageSize = pageSize;
        }

        void UpdatePagePosition(bool sizeChanged) {
            if (sizeChanged) {
                targetPosition = GetItemStartPosition(targetItemIndex);
                position = targetPosition;
                ClampPosition();
                ClampTargetItemIndex();
            }

            if (position == targetPosition) {
                return;
            }

            float num = (itemSize + spacing) / itemScrollTime;

            if (position < targetPosition) {
                position += num * Time.deltaTime;

                if (position > targetPosition) {
                    position = targetPosition;
                }
            } else {
                position -= num * Time.deltaTime;

                if (position < targetPosition) {
                    position = targetPosition;
                }
            }
        }

        void UpdateVisibility(IndexRange newVisibleItemsRange, IndexRange newScreenRange, bool canDestroyImmediate) {
            IndexRange removedLow;
            IndexRange removedHigh;
            IndexRange addedLow;
            IndexRange addedHigh;

            visibleItemsRange.CalculateDifference(newVisibleItemsRange,
                out removedLow,
                out removedHigh,
                out addedLow,
                out addedHigh);

            if (!canDestroyImmediate) { }

            if (newVisibleItemsRange != visibleItemsRange) {
                IndexRange prevVisibleItemsRange = visibleItemsRange;
                visibleItemsRange = newVisibleItemsRange;
                int i = removedLow.Position;
                int num = 0;

                for (; i <= removedLow.EndPosition; i++) {
                    if (!canDestroyImmediate) {
                        num++;
                    }

                    OnItemInvisible(num, canDestroyImmediate);
                }

                int num2 = removedHigh.EndPosition;
                int num3 = rectTransform.childCount - 1;

                while (num2 >= removedHigh.Position) {
                    OnItemInvisible(num3, canDestroyImmediate);
                    num3--;
                    num2--;
                }

                int num4 = addedLow.Position;
                int num5 = 0;

                while (num4 <= addedLow.EndPosition) {
                    OnItemVisible(num5);
                    num4++;
                    num5++;
                }

                for (int j = addedHigh.Position; j <= addedHigh.EndPosition; j++) {
                    OnItemVisible(rectTransform.childCount);
                }

                if (addedLow.Contains(selectedItemIndex) || addedHigh.Contains(selectedItemIndex)) {
                    RectTransform item = GetItem(selectedItemIndex);
                    item.GetComponent<ListItem>().PlaySelectionAnimation();
                }

                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                    engine.ScheduleEvent(new ItemsVisibilityChangedEvent(prevVisibleItemsRange, visibleItemsRange),
                        entityBehaviour.Entity);
                });
            }

            if (newScreenRange != screenRange) {
                screenRange = newScreenRange;

                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                    engine.ScheduleEvent(new ScreenRangeChangedEvent(screenRange), entityBehaviour.Entity);
                });
            }
        }

        IndexRange CalculateVisibleItemsRange() {
            if (itemsPerPage == 0) {
                return default;
            }

            int num = PositionToItemIndex(position + 0.01f);
            int num2 = PositionToItemIndex(position + pageSize - 0.01f);

            if (num >= 0 && num2 == -1) {
                num2 = itemsCount - 1;
            }

            return IndexRange.CreateFromBeginAndEnd(num, num2);
        }

        IndexRange CalculateScreenRange() {
            int num = PositionToItemIndexUnclamped(position + 0.01f);
            int endPosition = PositionToItemIndexUnclamped(position + pageSize - 0.01f);
            return IndexRange.CreateFromBeginAndEnd(num, endPosition);
        }

        void UpdateItemsPositionsAndSizes() {
            int num = visibleItemsRange.Position;
            int num2 = 0;

            while (num <= visibleItemsRange.EndPosition) {
                RectTransform rectTransform = (RectTransform)this.rectTransform.GetChild(num2);
                Vector2 sizeDelta = rectTransform.sizeDelta;
                sizeDelta[GetAxis()] = itemSize;
                rectTransform.sizeDelta = sizeDelta;
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition[GetAxis()] = GetItemCenterPosition(num) - position;
                rectTransform.anchoredPosition = anchoredPosition;
                num++;
                num2++;
            }
        }

        void SendScrollLimitEvent() {
            bool flag = position <= 0.01f;
            bool flag2 = position >= allContentSize - pageSize - 0.01f;

            if (flag != AtLimitLow || flag2 != AtLimitHigh) {
                AtLimitLow = flag;
                AtLimitHigh = flag2;

                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                    engine.ScheduleEvent<ScrollLimitEvent>(entityBehaviour.Entity);
                });
            }
        }

        void OnItemSelect(ListItem listItem) {
            SelectItem(listItem);
            SelectItemContent();
        }

        void SelectItem(ListItem listItem) {
            int num = selectedItemIndex;
            selectedItemIndex = ChildIndexToItemIndex(listItem.transform.GetSiblingIndex());
            log.DebugFormat("SelectItem prevSelectedItemIndex={0} selectedItemIndex={1}", num, selectedItemIndex);
            RectTransform item = GetItem(num);

            if (item != null) {
                item.GetComponent<ListItem>().PlayDeselectionAnimation();
            }

            RectTransform item2 = GetItem(selectedItemIndex);

            if (item2 != null) {
                item2.GetComponent<ListItem>().PlaySelectionAnimation();
            }
        }

        void SelectItemContent() {
            log.DebugFormat("SelectItemContent OUTER selectedItemIndex={0}", selectedItemIndex);
            RectTransform itemContent = GetItemContent(selectedItemIndex);
            Entity entity = GetEntity(itemContent);

            if (entity == selectedEntity) {
                return;
            }

            UnselectItemContent();

            if (entity != null) {
                selectedEntity = entity;

                ClientUnityIntegrationUtils.ExecuteInFlow(delegate {
                    log.DebugFormat("SelectItemContent INNER selectedEntity={0}", entity);
                    entity.AddComponent<SelectedListItemComponent>();
                });
            }
        }

        void UnselectItemContent() {
            log.DebugFormat("UnselectItemContent OUTER selectedEntity={0}", selectedEntity);

            if (selectedEntity == null) {
                return;
            }

            Entity entity = selectedEntity;
            selectedEntity = null;

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate {
                log.DebugFormat("UnselectItemContent INNER selectedEntity={0}", entity);

                if (entity.HasComponent<SelectedListItemComponent>() && !entity.HasComponent<DeletedEntityComponent>()) {
                    entity.RemoveComponent<SelectedListItemComponent>();
                }
            });
        }

        void OnItemInvisible(int childIndex, bool canDestroyImmediate) {
            log.DebugFormat("OnItemInvisible childIndex={0}", childIndex);
            Transform child = rectTransform.GetChild(childIndex);

            if (canDestroyImmediate) {
                DestroyImmediate(child.gameObject);
            } else {
                Destroy(child.gameObject);
            }
        }

        void OnItemVisible(int childIndex) {
            log.DebugFormat("OnItemVisible childIndex={0}", childIndex);
            ListItem listItem = CreateItem();
            listItem.transform.SetSiblingIndex(childIndex);
        }

        bool AtTarget() => position == targetPosition;

        void ClampPosition() {
            if (targetPosition > allContentSize - pageSize) {
                targetPosition = allContentSize - pageSize;

                if (position > targetPosition) {
                    position = targetPosition;
                }
            }

            if (targetPosition < 0f) {
                targetPosition = 0f;

                if (position < 0f) {
                    position = 0f;
                }
            }
        }

        void ClampTargetItemIndex() {
            if (targetItemIndex + itemsPerPage > itemsCount) {
                targetItemIndex = itemsCount - itemsPerPage;
            }

            if (targetItemIndex < 0) {
                targetItemIndex = 0;
            }
        }

        int ItemIndexToChildIndex(int itemIndex) {
            if (!IsItemVisible(itemIndex)) {
                return -1;
            }

            return itemIndex - visibleItemsRange.Position;
        }

        bool IsItemVisible(int itemIndex) => visibleItemsRange.Contains(itemIndex);

        int ChildIndexToItemIndex(int childIndex) => childIndex + visibleItemsRange.Position;

        public RectTransform GetItem(int itemIndex) {
            int num = ItemIndexToChildIndex(itemIndex);

            if (num != -1) {
                return (RectTransform)rectTransform.GetChild(num);
            }

            return null;
        }

        ListItem CreateItem() {
            ListItem listItem = Instantiate(itemPrefab);
            listItem.transform.SetParent(rectTransform, false);
            return listItem;
        }

        static Entity GetEntity(Transform content) {
            if (content != null) {
                EntityBehaviour component = content.GetComponent<EntityBehaviour>();

                if (component != null) {
                    return component.Entity;
                }
            }

            return null;
        }

        int PositionToItemIndex(float pos) {
            if (itemsCount == 0) {
                return -1;
            }

            float num = itemSize + spacing;
            int num2 = -1;

            if (pos >= num) {
                num2 = 1 + Mathf.FloorToInt((pos - num) / num);
            } else if (pos > 0f) {
                num2 = 0;
            }

            if (num2 > itemsCount - 1) {
                num2 = -1;
            }

            return num2;
        }

        int PositionToItemIndexUnclamped(float pos) {
            if (itemsPerPage == 0) {
                return 0;
            }

            float num = itemSize + spacing;
            float f = pos / num;
            return Mathf.FloorToInt(f);
        }

        float GetItemCenterPosition(int itemIndex) => GetItemStartPosition(itemIndex) + itemSize / 2f;

        float GetItemStartPosition(int itemIndex) => itemIndex * (itemSize + spacing);

        float GetSize() {
            float num = rectTransform.rect.size[GetAxis()];

            if (num < 0f) {
                return 0f;
            }

            return num;
        }

        int GetAxis() => vertical ? 1 : 0;

        static float GetAllContentSize(int itemsCount, float itemSize, float spacing) {
            float num = itemsCount * (itemSize + spacing);

            if (itemsCount > 1) {
                num -= spacing;
            }

            return num;
        }

        static float GetItemSize(float pageSize, int itemsPerPage, float spacing) {
            if (itemsPerPage > 0) {
                return (pageSize - spacing * (itemsPerPage - 1)) / itemsPerPage;
            }

            return 0f;
        }

        static int GetItemsPerPage(float itemsMinSize, float spacing, float pageSize) {
            if (itemsMinSize > 0f) {
                if (itemsMinSize <= pageSize) {
                    float num = pageSize - itemsMinSize;
                    float num2 = itemsMinSize + spacing;
                    return 1 + Mathf.FloorToInt(num / num2);
                }
            }

            return 0;
        }
    }
}