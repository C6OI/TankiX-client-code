using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PresetListComponent : UIBehaviour, Component, IScrollHandler, IEventSystemHandler {
        [SerializeField] GameObject elementPrefab;

        [SerializeField] RectTransform contentRoot;

        [SerializeField] Button leftButton;

        [SerializeField] Button rightButton;

        [SerializeField] float speed = 100f;

        [SerializeField] GameObject buyButton;

        [SerializeField] GaragePrice xBuyPrice;

        [SerializeField] Animator animator;

        [SerializeField] LocalizedField lockedByRankLocalizedText;

        [SerializeField] TextMeshProUGUI lockedByRankText;

        bool immediate;

        float targetPos;

        [Inject] public static EngineService EngineService { get; set; }

        public GameObject BuyButton => buyButton;

        public GaragePrice XBuyPrice => xBuyPrice;

        public BuyConfirmationDialog BuyDialog => FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>();

        public LocalizedField LockedByRankLocalizedText => lockedByRankLocalizedText;

        public string LockedByRankText {
            set => lockedByRankText.text = value;
        }

        public RectTransform ContentRoot => contentRoot;

        public int ElementsCount => contentRoot.transform.childCount;

        public int SelectedItemIndex { get; private set; }

        protected override void Awake() {
            leftButton.onClick.AddListener(ScrollLeft);
            rightButton.onClick.AddListener(ScrollRight);
        }

        void Update() {
            if (immediate) {
                targetPos = GetElementPos(SelectedItemIndex);
            }

            if (!Mathf.Approximately(GetPos(), targetPos)) {
                float b = speed * Time.deltaTime;
                float num = targetPos - GetPos();

                if (immediate) {
                    Move(num);
                    immediate = false;
                } else {
                    float deltaPos = !(num >= 0f) ? 0f - Mathf.Min(0f - num, b) : Mathf.Min(num, b);
                    Move(deltaPos);
                }
            }

            if (Input.GetButtonDown("MoveRight")) {
                if (!InputFieldComponent.IsAnyInputFieldInFocus()) {
                    ScrollRight();
                }
            } else if (Input.GetButtonDown("MoveLeft") && !InputFieldComponent.IsAnyInputFieldInFocus()) {
                ScrollLeft();
            }

            if (TutorialCanvas.Instance.IsShow) {
                if (leftButton.gameObject.activeSelf) {
                    leftButton.gameObject.SetActive(false);
                }

                if (rightButton.gameObject.activeSelf) {
                    rightButton.gameObject.SetActive(false);
                }

                return;
            }

            float num2 = 1E-06f;
            bool flag = ElementsCount > 1 && GetPos() > GetElementPos(1) - num2;

            if (flag != leftButton.gameObject.activeSelf) {
                leftButton.gameObject.SetActive(flag);
            }

            bool flag2 = ElementsCount > 1 && GetPos() < GetElementPos(ElementsCount - 2) + num2;

            if (flag2 != rightButton.gameObject.activeSelf) {
                rightButton.gameObject.SetActive(flag2);
            }
        }

        public void OnScroll(PointerEventData eventData) {
            if (eventData.scrollDelta.y < 0f) {
                ScrollRight();
            } else if (eventData.scrollDelta.y > 0f) {
                ScrollLeft();
            }
        }

        public void SetBuyButtonEnabled(bool enabled) {
            animator.SetBool("BuyEnabled", enabled);
        }

        public void SetLockedByRankTextEnabled(bool enabled) {
            animator.SetBool("LockedByRankTextEnabled", enabled);
        }

        public GameObject AddElement() {
            bool flag = ElementsCount == 0;
            GameObject result = Instantiate(elementPrefab, contentRoot, false);

            if (flag) {
                SendSelectedEvent(0);
            }

            return result;
        }

        public void RemoveElement(int index) {
            if (index == SelectedItemIndex) {
                if (index > 0) {
                    Scroll(-1, true);
                } else {
                    Scroll(1, true);
                }
            } else if (index < SelectedItemIndex && SelectedItemIndex > 0) {
                SelectedItemIndex--;
            }

            Destroy(GetChild(index).gameObject);
        }

        public void Clear() {
            contentRoot.DestroyChildren();
        }

        public void ScrollLeft() {
            Scroll(-1, false);
        }

        public void ScrollRight() {
            Scroll(1, false);
        }

        public void Scroll(int delta, bool immediate) {
            int nextElement = GetNextElement(delta);
            ScrollToElement(nextElement, immediate, true);
        }

        public void ScrollToElement(int elementIndex, bool immediate, bool sendSelected) {
            SelectedItemIndex = elementIndex;
            targetPos = GetElementPos(SelectedItemIndex);
            this.immediate = immediate;

            if (immediate) {
                float deltaPos = targetPos - GetPos();
                Move(deltaPos);
            }

            if (sendSelected) {
                SendSelectedEvent(SelectedItemIndex);
            }
        }

        void SendSelectedEvent(int elementIndex) {
            EntityBehaviour component = GetChild(elementIndex).GetComponent<EntityBehaviour>();

            if (component != null && component.Entity != null) {
                EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>(component.Entity);
            }
        }

        int GetNextElement(int delta) {
            int num = SelectedItemIndex + delta;

            if (num >= ElementsCount) {
                num = ElementsCount - 1;
            }

            if (num < 0) {
                num = 0;
            }

            return num;
        }

        float GetElementPos(int index) {
            if (ElementsCount == 0) {
                return 0f;
            }

            return GetChild(index).anchoredPosition.x;
        }

        float GetPos() => 0f - contentRoot.anchoredPosition.x;

        void Move(float deltaPos) {
            Vector2 anchoredPosition = contentRoot.anchoredPosition;
            anchoredPosition.x -= deltaPos;
            contentRoot.anchoredPosition = anchoredPosition;
        }

        RectTransform GetChild(int index) => (RectTransform)contentRoot.transform.GetChild(index);
    }
}