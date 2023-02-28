using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Graphic))]
    public class UITint : MonoBehaviour {
        UITintController controller;

        Graphic graphic;

        void Awake() {
            graphic = GetComponent<Graphic>();
        }

        void OnEnable() {
            ClearController();
            InitController();
        }

        void OnDisable() {
            ClearController();
        }

        void OnTransformParentChanged() {
            ClearController();
            InitController();
        }

        void InitController() {
            UITintController componentInParent = GetComponentInParent<UITintController>();

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

        public virtual void SetTint(Color tint) {
            graphic.color = tint;
        }
    }
}