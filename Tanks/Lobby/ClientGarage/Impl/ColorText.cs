using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Text))]
    public class ColorText : MonoBehaviour, IColorButtonElement {
        public bool noApplyMaterial;

        public Text text;

        ColorButtonController controller;

        void Awake() => text = GetComponent<Text>();

        void OnEnable() {
            ClearController();
            InitController();
        }

        void OnDisable() => ClearController();

        void OnTransformParentChanged() {
            ClearController();
            InitController();
        }

        public virtual void SetColor(ColorData color) {
            text.color = color.color;

            if (!noApplyMaterial) {
                text.material = color.material;
            }
        }

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