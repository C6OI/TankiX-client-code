using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lobby.ClientControls.API {
    public static class SimpleLayoutUtility {
        static readonly List<Component> components = new(3);

        public static float GetFlexibleSize(RectTransform rect, int axis) {
            if (axis == 0) {
                return GetFlexibleWidth(rect);
            }

            return GetFlexibleHeight(rect);
        }

        public static float GetMinSize(RectTransform rect, int axis) {
            if (axis == 0) {
                return GetMinWidth(rect);
            }

            return GetMinHeight(rect);
        }

        public static float GetMaxSize(RectTransform rect, int axis) {
            if (axis == 0) {
                return GetMaxWidth(rect);
            }

            return GetMaxHeight(rect);
        }

        public static float GetFlexibleWidth(RectTransform rect) => GetLayoutProperty(rect, e => e.flexibleWidth, 0f);

        public static float GetFlexibleHeight(RectTransform rect) => GetLayoutProperty(rect, e => e.flexibleHeight, 0f);

        public static float GetMinWidth(RectTransform rect) => GetLayoutProperty(rect, e => e.minWidth, 0f);

        public static float GetMaxWidth(RectTransform rect) => GetLayoutProperty(rect, e => e.maxWidth, 0f);

        public static float GetMinHeight(RectTransform rect) => GetLayoutProperty(rect, e => e.minHeight, 0f);

        public static float GetMaxHeight(RectTransform rect) => GetLayoutProperty(rect, e => e.maxHeight, 0f);

        public static float GetLayoutProperty(RectTransform rect, Func<ISimpleLayoutElement, float> property,
            float defaultValue) {
            ISimpleLayoutElement source;
            return GetLayoutProperty(rect, property, defaultValue, out source);
        }

        public static float GetLayoutProperty(RectTransform rect, Func<ISimpleLayoutElement, float> property,
            float defaultValue, out ISimpleLayoutElement source) {
            source = null;

            if (rect == null) {
                return 0f;
            }

            float num = defaultValue;
            int num2 = int.MinValue;
            rect.GetComponents(typeof(ISimpleLayoutElement), components);

            for (int i = 0; i < components.Count; i++) {
                ISimpleLayoutElement simpleLayoutElement = components[i] as ISimpleLayoutElement;

                if (simpleLayoutElement is Behaviour &&
                    (!(simpleLayoutElement as Behaviour).enabled ||
                     !(simpleLayoutElement as Behaviour).isActiveAndEnabled)) {
                    continue;
                }

                int layoutPriority = simpleLayoutElement.layoutPriority;

                if (layoutPriority < num2) {
                    continue;
                }

                float num3 = property(simpleLayoutElement);

                if (!(num3 < 0f)) {
                    if (layoutPriority > num2) {
                        num = num3;
                        num2 = layoutPriority;
                        source = simpleLayoutElement;
                    } else if (num3 > num) {
                        num = num3;
                        source = simpleLayoutElement;
                    }
                }
            }

            components.Clear();
            return num;
        }
    }
}