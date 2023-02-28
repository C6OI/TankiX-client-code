using System.Collections;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl.Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SlotView : MonoBehaviour {
        public int moduleCard3DScale;

        public TooltipShowBehaviour tooltip;

        public Image dropInnerGlow;

        public Image dropOuterGlow;

        DragAndDropCell dragAndDropCell;

        void Awake() {
            dragAndDropCell = GetComponent<DragAndDropCell>();

            if ((bool)dropInnerGlow) {
                dropInnerGlow.gameObject.SetActive(false);
            }

            if ((bool)dropOuterGlow) {
                dropOuterGlow.gameObject.SetActive(false);
            }
        }

        public virtual void SetItem(SlotItemView item) {
            item.transform.SetParent(transform, false);
            UpdateItemTransform(item);
        }

        protected void UpdateItemTransform(SlotItemView item) {
            item.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            item.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            item.SetScaleToCard3D(moduleCard3DScale);
        }

        public void OnItemPlace(DragAndDropItem item) {
            SlotItemView component = item.GetComponent<SlotItemView>();
            UpdateItemTransform(component);
            component.HighlightEnable = true;
        }

        public void TurnOnRenderAboveAll() {
            if (!ModulesTutorialUtil.TUTORIAL_MODE) {
                StartCoroutine(DelayedTurnOnRenderAboveAll());
            }
        }

        IEnumerator DelayedTurnOnRenderAboveAll() {
            yield return new WaitForEndOfFrame();

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector3 pos = rectTransform.anchoredPosition3D;
            pos.z = NewModulesScreenUIComponent.OVER_SCREEN_Z_OFFSET;
            rectTransform.anchoredPosition3D = pos;
            Canvas canvas = gameObject.GetComponent<Canvas>();

            if (canvas == null) {
                canvas = gameObject.AddComponent<Canvas>();
            }

            canvas.renderMode = RenderMode.WorldSpace;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 30;
            gameObject.AddComponent<GraphicRaycaster>();
            CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = true;
            canvasGroup.ignoreParentGroups = true;
            canvasGroup.interactable = false;
        }

        public void TurnOffRenderAboveAll() {
            StopAllCoroutines();

            if (!ModulesTutorialUtil.TUTORIAL_MODE && gameObject.GetComponent<Canvas>() != null) {
                Destroy(gameObject.GetComponent<GraphicRaycaster>());
                Destroy(gameObject.GetComponent<Canvas>());
                Destroy(gameObject.GetComponent<CanvasGroup>());
                RectTransform component = gameObject.GetComponent<RectTransform>();
                Vector3 anchoredPosition3D = component.anchoredPosition3D;
                anchoredPosition3D.z = 0f;
                component.anchoredPosition3D = anchoredPosition3D;
            }
        }

        public void HighlightForDrop() {
            SlotItemView item = GetItem();

            if (item != null) {
                dropOuterGlow.gameObject.SetActive(true);
                item.HighlightEnable = false;
            } else {
                dropInnerGlow.gameObject.SetActive(true);
            }
        }

        public void CancelHighlightForDrop() {
            dropInnerGlow.gameObject.SetActive(false);
            dropOuterGlow.gameObject.SetActive(false);
            SlotItemView item = GetItem();

            if (item != null) {
                item.HighlightEnable = true;
            }
        }

        public bool HasItem() => dragAndDropCell.GetItem() != null;

        public SlotItemView GetItem() {
            DragAndDropItem item = dragAndDropCell.GetItem();
            return !(item == null) ? item.GetComponent<SlotItemView>() : null;
        }
    }
}