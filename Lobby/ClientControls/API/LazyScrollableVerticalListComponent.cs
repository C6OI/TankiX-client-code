using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(ScrollRect))]
    public class LazyScrollableVerticalListComponent : MonoBehaviour, Component {
        static readonly Vector3[] corners = new Vector3[4];

        LayoutElement aboveViewportPlaceholder;

        LayoutElement belowViewportPlaceholder;
        ScrollRect scrollRect;

        void Awake() {
            scrollRect = GetComponent<ScrollRect>();
            CreatePlaceholders();
        }

        void Start() => scrollRect.onValueChanged.AddListener(OnValueChange);

        void OnDisable() {
            aboveViewportPlaceholder.minHeight = 0f;
            aboveViewportPlaceholder.preferredHeight = 0f;
            belowViewportPlaceholder.minHeight = 0f;
            belowViewportPlaceholder.preferredHeight = 0f;
        }

        void CreatePlaceholders() {
            aboveViewportPlaceholder = CreatePlacehodler("ABOVE_PLACEHOLDER");
            belowViewportPlaceholder = CreatePlacehodler("BELOW_PLACEHOLDER");
            AdjustPlaceholdersSiblingIndices();
        }

        LayoutElement CreatePlacehodler(string name) {
            LayoutElement layoutElement = new GameObject(name).AddComponent<LayoutElement>();
            RectTransform component = layoutElement.gameObject.GetComponent<RectTransform>();
            component.SetParent(scrollRect.content, false);
            component.pivot = new Vector2(0f, 1f);
            layoutElement.minHeight = 0f;
            layoutElement.preferredHeight = 0f;
            return layoutElement;
        }

        void OnValueChange(Vector2 value) => AdjustChildrenVisibility();

        public void AdjustChildrenVisibility() {
            AdjustPlaceholdersSiblingIndices();
            float spacing = scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing;
            int num = 0;
            float num2 = 0f;

            while (num < scrollRect.content.childCount) {
                RectTransform rectTransform = (RectTransform)scrollRect.content.GetChild(num);

                if (IsPlaceholder(rectTransform)) {
                    num++;
                    continue;
                }

                AlignInactiveChild(rectTransform, num2);

                if (AreTransformsOverlaps(rectTransform, scrollRect.viewport)) {
                    break;
                }

                num2 += rectTransform.rect.height + spacing;

                if (rectTransform.gameObject.activeSelf) {
                    rectTransform.gameObject.SetActive(false);
                }

                num++;
            }

            SetPlaceholderHeight(aboveViewportPlaceholder, !(num2 > 0f) ? 0f : num2 - spacing);

            while (num < scrollRect.content.childCount) {
                RectTransform rectTransform2 = (RectTransform)scrollRect.content.GetChild(num);

                if (IsPlaceholder(rectTransform2)) {
                    num++;
                    continue;
                }

                AlignInactiveChild(rectTransform2, num2);

                if (!AreTransformsOverlaps(rectTransform2, scrollRect.viewport)) {
                    break;
                }

                num2 += rectTransform2.rect.height + spacing;

                if (!rectTransform2.gameObject.activeSelf) {
                    rectTransform2.gameObject.SetActive(true);
                }

                num++;
            }

            float num3 = 0f;
            RectTransform rectTransform3 = (RectTransform)belowViewportPlaceholder.transform;

            while (num < scrollRect.content.childCount) {
                RectTransform rectTransform4 = (RectTransform)scrollRect.content.GetChild(num);

                if (IsPlaceholder(rectTransform4)) {
                    num++;
                    continue;
                }

                AlignInactiveChild(rectTransform4, 0f - rectTransform3.anchoredPosition.y + num3);
                num3 += rectTransform4.rect.height + spacing;

                if (rectTransform4.gameObject.activeSelf) {
                    rectTransform4.gameObject.SetActive(false);
                }

                num++;
            }

            SetPlaceholderHeight(belowViewportPlaceholder, !(num3 > 0f) ? 0f : num3 - spacing);
        }

        void SetPlaceholderHeight(LayoutElement placeholder, float height) {
            placeholder.minHeight = height;
            placeholder.preferredHeight = placeholder.preferredHeight;
            placeholder.gameObject.SetActive(placeholder.minHeight > 0f);
        }

        void AlignInactiveChild(RectTransform inactiveItem, float offset) {
            if (!inactiveItem.gameObject.activeSelf) {
                inactiveItem.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, offset, inactiveItem.rect.height);
            }
        }

        public void AdjustPlaceholdersSiblingIndices() {
            aboveViewportPlaceholder.transform.SetAsFirstSibling();
            belowViewportPlaceholder.transform.SetAsLastSibling();
        }

        bool IsPlaceholder(RectTransform transform) => transform.gameObject == aboveViewportPlaceholder.gameObject ||
                                                       transform.gameObject == belowViewportPlaceholder.gameObject;

        static bool AreTransformsOverlaps(RectTransform a, RectTransform b) {
            a.GetWorldCorners(corners);
            Rect rect = new(corners[0], corners[2] - corners[0]);
            b.GetWorldCorners(corners);
            Rect other = new(corners[0], corners[2] - corners[0]);
            return rect.Overlaps(other);
        }
    }
}