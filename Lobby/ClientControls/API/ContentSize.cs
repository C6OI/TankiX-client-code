using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class ContentSize : UIBehaviour {
        public Vector2 offsets;

        public bool constantWidth;

        RectTransform canvasRectTransform;

        Rect lastCanvasRect;

        RectTransform CanvasRectTransform {
            get {
                if (canvasRectTransform == null) {
                    Canvas componentInParent = GetComponentInParent<Canvas>();

                    if (componentInParent != null) {
                        canvasRectTransform = componentInParent.GetComponent<RectTransform>();
                    }
                }

                return canvasRectTransform;
            }
        }

        void Update() {
            if (lastCanvasRect != CanvasRectTransform.rect) {
                lastCanvasRect = CanvasRectTransform.rect;
                Validate();
            }
        }

        protected override void OnEnable() => Validate();

        protected override void OnRectTransformDimensionsChange() => Validate();

        protected override void OnTransformParentChanged() => canvasRectTransform = null;

        void Validate() {
            RectTransform rectTransform = CanvasRectTransform;

            if (!(rectTransform == null)) {
                RectTransform component = GetComponent<RectTransform>();
                Vector2 sizeDelta = component.sizeDelta;

                if (!constantWidth) {
                    sizeDelta.x = rectTransform.rect.width - offsets.x;
                }

                sizeDelta.y = rectTransform.rect.height - offsets.y;
                component.sizeDelta = sizeDelta;
            }
        }
    }
}