using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSelectScreenLayout : UIBehaviour, ILayoutSelfController, ILayoutController {
        public RectTransform scoreTableParent;

        public void SetLayoutHorizontal() { }

        public void SetLayoutVertical() {
            if (scoreTableParent.childCount > 0) {
                RectTransform component = GetComponent<RectTransform>();
                RectTransform rectTransform = (RectTransform)scoreTableParent.GetChild(0);
                float height = rectTransform.rect.height;

                if (component.sizeDelta.y != height) {
                    component.sizeDelta = new Vector2(component.sizeDelta.x, height);
                }
            }
        }
    }
}