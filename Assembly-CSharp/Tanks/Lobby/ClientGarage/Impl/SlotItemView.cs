using System;
using Tanks.Lobby.ClientControls.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SlotItemView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        public GameObject moduleCard3DPrefab;

        public GameObject itemContent;

        public TooltipShowBehaviour tooltip;

        public Animator outline;

        public Color pressedColor;

        public Color highlidhtedColor;

        public Color upgradeColor;

        public float selectionSaturation = 1f;

        public float highlightedSaturation = 0.1f;

        bool highlightEnable = true;

        ModuleCardView moduleCard3D;

        public Action<SlotItemView> onClick;

        public Action<SlotItemView> onDoubleClick;

        bool selected;

        public bool HighlightEnable {
            get => highlightEnable;
            set {
                highlightEnable = value;
                UpdateHighlight();
            }
        }

        public ModuleItem ModuleItem { get; private set; }

        public void Awake() {
            GameObject gameObject = Instantiate(moduleCard3DPrefab);
            gameObject.transform.SetParent(itemContent.transform, false);
            gameObject.transform.position = Vector3.zero;
            moduleCard3D = gameObject.GetComponent<ModuleCardView>();
        }

        void OnEnable() {
            if (ModuleItem != null) {
                UpdateHighlight();
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Right) {
                if (onClick != null && eventData.clickCount == 1) {
                    onClick(this);
                }

                if (onDoubleClick != null && eventData.clickCount > 1) {
                    onDoubleClick(this);
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Right && !selected && highlightEnable) {
                HighlightPressed();
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (!selected && highlightEnable) {
                HighlightHighlighted();
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (!selected) {
                UpdateHighlight();
            }
        }

        public void SetScaleToCard3D(float scale) {
            moduleCard3D.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void UpdateView(ModuleItem moduleItem) {
            ModuleItem = moduleItem;
            moduleCard3D.UpdateView(moduleItem.MarketItem.Id);
            UpdateHighlight();
            tooltip.SetCustomContentData(moduleItem);
        }

        public void UpdateHighlight() {
            if (selected) {
                HighlightPressed();
            } else if (!highlightEnable) {
                HideHighlight();
            } else if (ModuleItem.ImproveAvailable()) {
                HighlightUpgrade();
            } else {
                HideHighlight();
            }
        }

        public void Select() {
            selected = true;
            UpdateHighlight();
        }

        public void Deselect() {
            selected = false;
            UpdateHighlight();
        }

        void HighlightUpgrade() {
            outline.SetInteger("colorCode", 3);
        }

        void HighlightPressed() {
            outline.SetInteger("colorCode", 1);
        }

        void HighlightHighlighted() {
            outline.SetInteger("colorCode", 2);
        }

        void HideHighlight() {
            outline.SetInteger("colorCode", 0);
        }
    }
}