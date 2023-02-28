using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class DependentSize : ResizeListener, ILayoutElement {
        public bool useMinWidth;

        public bool usePreferredWidth;

        public bool useFlexibleWidth;

        public bool useMinHeight;

        public bool usePreferredHeight;

        public bool useFlexibleHeight;

        RectTransform layoutSource;

        public float minWidth { get; private set; }

        public float preferredWidth { get; private set; }

        public float flexibleWidth { get; private set; }

        public float minHeight { get; private set; }

        public float preferredHeight { get; private set; }

        public float flexibleHeight { get; private set; }

        public int layoutPriority => 0;

        public void CalculateLayoutInputHorizontal() {
            minWidth = GetValue(useMinWidth, () => LayoutUtility.GetMinWidth(layoutSource));
            flexibleWidth = GetValue(useFlexibleWidth, () => LayoutUtility.GetFlexibleWidth(layoutSource));
            preferredWidth = GetValue(usePreferredWidth, () => LayoutUtility.GetPreferredWidth(layoutSource));
        }

        public void CalculateLayoutInputVertical() {
            minHeight = GetValue(useMinHeight, () => LayoutUtility.GetMinHeight(layoutSource));
            flexibleHeight = GetValue(useFlexibleHeight, () => LayoutUtility.GetFlexibleHeight(layoutSource));
            preferredHeight = GetValue(usePreferredHeight, () => LayoutUtility.GetPreferredHeight(layoutSource));
        }

        public override void OnResize(RectTransform source) {
            layoutSource = source;
            CalculateLayoutInputHorizontal();
            CalculateLayoutInputVertical();
            LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
        }

        float GetValue(bool use, Func<float> value) => !use || !(layoutSource != null) ? -1f : value();
    }
}