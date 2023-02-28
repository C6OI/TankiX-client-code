using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class BackgroundDimensionsChangeComponent : UIBehaviour {
        [SerializeField] Image backgroundImage;

        protected override void OnEnable() {
            OnRectTransformDimensionsChange();
        }

        protected override void OnRectTransformDimensionsChange() {
            if (!(backgroundImage == null) && !(backgroundImage.overrideSprite == null)) {
                Rect rect = backgroundImage.overrideSprite.rect;
                float num = rect.width / rect.height;
                RectTransform rectTransform = (RectTransform)transform;
                RectTransform rectTransform2 = (RectTransform)backgroundImage.transform;
                float num2 = rectTransform.rect.width / rectTransform.rect.height;

                rectTransform2.sizeDelta = !(num2 < num) ? new Vector2(rectTransform.rect.width, rectTransform.rect.width / num)
                                               : new Vector2(num * rectTransform.rect.height, rectTransform.rect.height);
            }
        }
    }
}