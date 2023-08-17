using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientNavigation.Impl {
    [RequireComponent(typeof(Animator))]
    public class BackgroundComponent : UIBehaviour, Component {
        const string VISIBLE_ANIMATION_PARAM = "Visible";

        [SerializeField] Image backgroundImage;

        protected override void OnRectTransformDimensionsChange() {
            if (!(backgroundImage.overrideSprite == null)) {
                Rect rect = backgroundImage.overrideSprite.rect;
                float num = rect.width / rect.height;
                RectTransform rectTransform = (RectTransform)transform;
                RectTransform rectTransform2 = (RectTransform)backgroundImage.transform;
                float num2 = rectTransform.rect.width / rectTransform.rect.height;

                rectTransform2.sizeDelta = !(num2 < num)
                                               ? new Vector2(rectTransform.rect.width, rectTransform.rect.width / num)
                                               : new Vector2(num * rectTransform.rect.height, rectTransform.rect.height);
            }
        }

        public virtual void Hide() => GetComponent<Animator>().SetBool("Visible", false);

        public virtual void Show() => GetComponent<Animator>().SetBool("Visible", true);
    }
}