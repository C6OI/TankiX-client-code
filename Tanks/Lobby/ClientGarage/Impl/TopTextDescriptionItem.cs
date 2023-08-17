using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    [RequireComponent(typeof(Text))]
    [ExecuteInEditMode]
    public class TopTextDescriptionItem : MonoBehaviour, ILayoutSelfController, ILayoutController {
        [SerializeField] public Scrollbar scroll;

        public string text {
            get => GetComponent<Text>().text;
            set {
                GetComponent<Text>().text = value;

                if (scroll != null) {
                    scroll.value = 1f;
                }

                LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
            }
        }

        public void SetLayoutHorizontal() { }

        public void SetLayoutVertical() {
            float y;
            float y2;
            float y3;

            if (scroll.gameObject.activeSelf) {
                y = 1f;
                y2 = 1f;
                y3 = 1f;
            } else {
                y = 0f;
                y2 = 0f;
                y3 = 0f;
            }

            RectTransform component = GetComponent<RectTransform>();
            Vector2 anchorMin = component.anchorMin;
            Vector2 anchorMax = component.anchorMax;
            anchorMin.y = y;
            anchorMax.y = y2;
            component.anchorMin = anchorMin;
            component.anchorMax = anchorMax;
            Vector2 pivot = component.pivot;
            pivot.y = y3;
            component.pivot = pivot;
        }
    }
}