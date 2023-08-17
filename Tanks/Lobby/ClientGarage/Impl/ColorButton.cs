using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class ColorButton : MonoBehaviour, IColorButtonElement {
        public bool noApplyMaterial;

        public bool hardlight;

        ColorButtonController controller;

        Image image;

        void Awake() => InitElement();

        void OnEnable() {
            InitElement();
            ClearController();
            InitController();
        }

        void OnDisable() => ClearController();

        void OnTransformParentChanged() {
            ClearController();
            InitController();
        }

        public void SetColor(ColorData colorData) {
            if (hardlight) {
                image.color = colorData.hardlightColor;
            } else {
                image.color = colorData.color;
            }

            if (!noApplyMaterial) {
                image.material = colorData.material;
            }
        }

        void InitElement() => image = GetComponent<Image>();

        void InitController() {
            ColorButtonController componentInParent = GetComponentInParent<ColorButtonController>();

            if (componentInParent != null) {
                componentInParent.AddElement(this);
            }

            controller = componentInParent;
        }

        void ClearController() {
            if (controller != null) {
                controller.RemoveElement(this);
            }

            controller = null;
        }
    }
}