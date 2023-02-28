using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Image))]
    public class TankBuilderMaterialAlphaController : BehaviourComponent {
        CanvasGroup[] canvasGroups;

        Material material;

        void Awake() {
            canvasGroups = GetComponentsInParent<CanvasGroup>();
            Image component = GetComponent<Image>();
            Material material2 = component.material = new Material(component.material);
            material = material2;
        }

        void Update() {
            SetAlpha();
        }

        void OnEnable() {
            SetAlpha();
        }

        void SetAlpha() {
            float num = 1f;
            CanvasGroup[] array = canvasGroups;

            foreach (CanvasGroup canvasGroup in array) {
                num *= canvasGroup.alpha;
            }

            material.SetFloat("alpha", 1f - num);
        }
    }
}