using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class UIRectClipper : MonoBehaviour, IClipper {
        [HideInInspector] [SerializeField] float fromX;

        [HideInInspector] [SerializeField] float toX = 1f;

        [SerializeField] RectTransform.Axis axis;

        readonly Vector3[] canvasCorners = new Vector3[4];

        readonly List<Canvas> canvases = new();

        readonly Vector3[] worldCorners = new Vector3[4];

        readonly List<MaskableGraphic> maskableCache = new();

        RectTransform rectTransform;

        public float FromX {
            get => fromX;
            set {
                fromX = value;
                PerformClipping();
            }
        }

        public float ToX {
            get => toX;
            set {
                toX = value;
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

            switch (axis) {
                case RectTransform.Axis.Horizontal: {
                    float width = canvasRect.width;
                    canvasRect.xMin = Mathf.Max(canvasRect.xMin, canvasRect.xMin + FromX * width);
                    canvasRect.xMax = Mathf.Min(canvasRect.xMax, canvasRect.xMax - (1f - ToX) * width);
                    break;
                }

                case RectTransform.Axis.Vertical: {
                    float height = canvasRect.height;
                    canvasRect.yMin = Mathf.Max(canvasRect.yMin, canvasRect.yMin + FromX * height);
                    canvasRect.yMax = Mathf.Min(canvasRect.yMax, canvasRect.yMax - (1f - ToX) * height);
                    break;
                }
            }

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