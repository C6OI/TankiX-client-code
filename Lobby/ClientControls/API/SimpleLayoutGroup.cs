using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public abstract class SimpleLayoutGroup : LayoutGroup {
        [SerializeField] protected float m_Spacing;

        readonly SimpleLayoutCalculator calculator = new();

        public float spacing {
            get => m_Spacing;
            set => SetProperty(ref m_Spacing, value);
        }

        protected void CalcAlongAxis(int axis, bool isVertical) {
            float num = axis != 0 ? padding.vertical : padding.horizontal;
            float num2 = num;
            float num3 = 0f;
            bool flag = isVertical ^ axis == 1;

            for (int i = 0; i < rectChildren.Count; i++) {
                RectTransform rect = rectChildren[i];
                float flexibleSize = SimpleLayoutUtility.GetFlexibleSize(rect, axis);
                float minSize = SimpleLayoutUtility.GetMinSize(rect, axis);

                if (flag) {
                    num3 = Mathf.Max(flexibleSize, num3);
                    num2 = Mathf.Max(minSize + num, num2);
                } else {
                    num3 += flexibleSize;
                    num2 += minSize + spacing;
                }
            }

            if (!flag && rectChildren.Count > 0) {
                num2 -= spacing;
            }

            float totalPreferred = num2;
            SetLayoutInputForAxis(num2, totalPreferred, num3, axis);
        }

        protected void SetChildrenAlongAxis(int axis, bool isVertical) {
            float num = rectTransform.rect.size[axis];

            if (isVertical ^ axis == 1) {
                float value = num - (axis != 0 ? padding.vertical : padding.horizontal);

                for (int i = 0; i < rectChildren.Count; i++) {
                    RectTransform rect = rectChildren[i];
                    float flexibleSize = SimpleLayoutUtility.GetFlexibleSize(rect, axis);
                    float minSize = SimpleLayoutUtility.GetMinSize(rect, axis);
                    float maxSize = SimpleLayoutUtility.GetMaxSize(rect, axis);
                    float num2 = minSize;
                    float num3 = Mathf.Clamp(value, minSize, !(flexibleSize > 0f) ? num2 : num);

                    if (maxSize > 0f) {
                        num3 = Mathf.Min(num3, maxSize);
                    }

                    float startOffset = GetStartOffset(axis, num3);
                    SetChildAlongAxis(rect, axis, startOffset, num3);
                }

                return;
            }

            float num4 = axis != 0 ? padding.top : padding.left;

            if (GetTotalFlexibleSize(axis) == 0f && GetTotalPreferredSize(axis) < num) {
                num4 = GetStartOffset(axis,
                    GetTotalPreferredSize(axis) - (axis != 0 ? padding.vertical : padding.horizontal));
            }

            calculator.Reset(rectChildren.Count);

            for (int j = 0; j < rectChildren.Count; j++) {
                RectTransform rect2 = rectChildren[j];
                float flexibleSize2 = SimpleLayoutUtility.GetFlexibleSize(rect2, axis);
                float minSize2 = SimpleLayoutUtility.GetMinSize(rect2, axis);
                float maxSize2 = SimpleLayoutUtility.GetMaxSize(rect2, axis);
                calculator.AddElement(flexibleSize2, minSize2, maxSize2);
            }

            float num5 = rectChildren.Count <= 1 ? 0f : (rectChildren.Count - 1) * spacing;
            calculator.Calculate(num - num5);

            for (int k = 0; k < rectChildren.Count; k++) {
                RectTransform rect3 = rectChildren[k];
                SimpleLayoutCalculator.Element element = calculator.elements[k];
                SetChildAlongAxis(rect3, axis, num4, element.size);
                num4 += element.size + spacing;
            }
        }
    }
}