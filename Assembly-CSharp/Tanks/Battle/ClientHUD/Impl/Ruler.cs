using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    [ExecuteInEditMode]
    public class Ruler : MonoBehaviour {
        [SerializeField] Image segment;

        [SerializeField] float spacing = 2f;

        public int segmentsCount = 1;

        public List<Image> segments = new();

        public Color Color;

        float fillAmount;

        Color imageColor;

        RectTransform rectTransform;

        public float Spacing {
            get => spacing;
            set => spacing = value;
        }

        public RectTransform RectTransform {
            get {
                if (rectTransform == null) {
                    rectTransform = GetComponent<RectTransform>();
                }

                return rectTransform;
            }
        }

        public float FillAmount {
            get => fillAmount;
            set {
                fillAmount = value;
                float num = 1f / segments.Count;

                for (int i = 0; i < segments.Count; i++) {
                    float t = Mathf.Clamp01((value - i * num) / num);
                    float segmentAnchorMin = GetSegmentAnchorMin(i);
                    float segmentAnchorMax = GetSegmentAnchorMax(i);
                    segments[i].rectTransform.anchorMax = new Vector2(Mathf.Lerp(segmentAnchorMin, segmentAnchorMax, t), 1f);
                }
            }
        }

        void Update() {
            if (imageColor != Color) {
                for (int i = 0; i < segments.Count; i++) {
                    segments[i].color = imageColor = Color;
                }
            }
        }

        public void UpdateSegments() {
            Clear();

            for (int i = 0; i < segmentsCount; i++) {
                Image image = Instantiate(segment, transform);
                image.rectTransform.anchorMin = new Vector2(GetSegmentAnchorMin(i), 0f);
                image.rectTransform.anchorMax = new Vector2(GetSegmentAnchorMax(i), 1f);
                RectTransform obj = image.rectTransform;
                Vector2 zero = Vector2.zero;
                image.rectTransform.offsetMax = zero;
                obj.offsetMin = zero;
                image.gameObject.SetActive(true);
                segments.Add(image);
            }
        }

        float GetSegmentAnchorMin(int i) {
            float width = RectTransform.rect.width;
            float num = width / segmentsCount;
            float num2 = spacing / width / 2f;
            return num / width * i + (i <= 0 ? 0f : num2);
        }

        float GetSegmentAnchorMax(int i) {
            float width = RectTransform.rect.width;
            float num = width / segmentsCount;
            float num2 = spacing / width / 2f;
            return num / width * (i + 1) - (i >= segmentsCount - 1 ? 0f : num2);
        }

        public void Clear() {
            imageColor = Color.clear;

            foreach (Image segment in segments) {
                if (segment != null) {
                    DestroyImmediate(segment.gameObject);
                }
            }

            segments.Clear();
        }
    }
}