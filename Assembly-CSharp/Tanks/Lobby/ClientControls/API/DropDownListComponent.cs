using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class DropDownListComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] protected TextMeshProUGUI listTitle;

        [SerializeField] protected DefaultListDataProvider dataProvider;

        [SerializeField] float maxHeight = 210f;

        bool isOpen;

        RectTransform listRect;
        public OnDropDownListItemSelected onDropDownListItemSelected;

        bool pointerOver;

        bool pointerOverContent;

        RectTransform scrollRectContent;

        public object Selected {
            get => dataProvider.Selected;
            set {
                dataProvider.Selected = value;
                listTitle.text = Selected.ToString();
            }
        }

        public int SelectionIndex {
            get => dataProvider.Data.IndexOf(Selected);
            set => Selected = dataProvider.Data[value];
        }

        protected bool IsOpen {
            get => isOpen;
            set {
                isOpen = value;
                CanvasGroup component = listRect.GetComponent<CanvasGroup>();
                component.alpha = !isOpen ? 0f : 1f;
                component.interactable = isOpen;
                component.blocksRaycasts = isOpen;
            }
        }

        void Awake() {
            ScrollRect componentInChildren = GetComponentInChildren<ScrollRect>();
            scrollRectContent = componentInChildren.content;
            listRect = componentInChildren.transform.parent.GetComponent<RectTransform>();
            GetComponent<Button>().onClick.AddListener(ClickAction);
            IsOpen = false;
        }

        void Update() {
            if (IsOpen) {
                float num = Mathf.Min(maxHeight, scrollRectContent.rect.height);

                if (listRect.sizeDelta.y != num) {
                    listRect.sizeDelta = new Vector2(listRect.sizeDelta.x, num);
                    scrollRectContent.anchoredPosition = Vector2.zero;
                    scrollRectContent.GetComponentInChildren<DynamicVerticalList>().ScrollToSelection();
                }

                if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !pointerOverContent && !pointerOver) {
                    IsOpen = false;
                }
            }

            pointerOverContent = false;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            pointerOver = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            pointerOver = false;
        }

        public void ClickAction() {
            IsOpen = !IsOpen;
        }

        protected virtual void OnItemSelect(ListItem item) {
            IsOpen = false;

            if (onDropDownListItemSelected != null) {
                onDropDownListItemSelected(item);
            }
        }

        protected virtual void PointerOverContentItem(ListItem item) {
            pointerOverContent = true;
        }
    }
}