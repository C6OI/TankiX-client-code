using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class UIRectClipperY : MonoBehaviour, IClipper {
        [HideInInspector] [SerializeField] float fromY;

        [HideInInspector] [SerializeField] float toY = 1f;

        readonly Vector3[] canvasCorners = new Vector3[4];

        readonly List<Canvas> canvases = new();

        readonly Vector3[] worldCorners = new Vector3[4];

        readonly List<MaskableGraphic> maskableCache = new();

        RectTransform rectTransform;

        public float FromY {
            get => fromY;
            set {
                fromY = value;
                PerformClipping();
            }
        }

        public float ToY {
            get => toY;
            set {
                toY = value;
                PerformClipping();
            }
        }

        RectTransform RectTransform {
            get {
                if (rectTransform == null) {
                    rectTransform = GetComponent<RectTransform>();
                }

                return rectTransform;
            }
        }

        void OnEnable() {
            ClipperRegistry.Register(this);
        }

        void OnDisable() {
            ClipperRegistry.Unregister(this);
        }

        public void PerformClipping() {
            Rect canvasRect = GetCanvasRect();
            float height = canvasRect.height;
            canvasRect.yMin = Mathf.Max(canvasRect.yMin, canvasRect.yMin + FromY * height);
            canvasRect.yMax = Mathf.Min(canvasRect.yMax, canvasRect.yMax - (1f - ToY) * height);
            CanvasRenderer component = GetComponent<CanvasRenderer>();

            if (component != null) {
                component.EnableRectClipping(canvasRect);
            }

            maskableCache.Clear();
            GetComponentsInChildren(maskableCache);

            foreach (MaskableGraphic item in maskableCache) {
                item.SetClipRect(canvasRect, true);
            }
        }

        public Rect GetCanvasRect() {
            Canvas canvas = null;
            gameObject.GetComponentsInParent(false, canvases);

            if (canvases.Count > 0) {
                canvas = canvases[canvases.Count - 1];
                canvases.Clear();
                RectTransform.GetWorldCorners(worldCorners);
                Transform transform = canvas.transform;

                for (int i = 0; i < 4; i++) {
                    canvasCorners[i] = transform.InverseTransformPoint(worldCorners[i]);
                }

                return new Rect(canvasCorners[0].x, canvasCorners[0].y, canvasCorners[2].x - canvasCorners[0].x, canvasCorners[2].y - canvasCorners[0].y);
            }

            return default;
        }
    }
}