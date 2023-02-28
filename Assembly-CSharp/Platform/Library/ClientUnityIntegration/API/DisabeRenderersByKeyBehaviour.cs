using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public class DisabeRenderersByKeyBehaviour : MonoBehaviour {
        public KeyCode keyKode;

        Renderer[] disabledRenderers = new Renderer[0];

        bool renderersDisabled;

        void Update() {
            if (Input.GetKeyDown(keyKode)) {
                renderersDisabled = !renderersDisabled;
            }

            if (renderersDisabled) {
                DisableRenderers();
            } else {
                EnabeRenderers();
            }
        }

        void EnabeRenderers() {
            Renderer[] array = disabledRenderers;

            foreach (Renderer renderer in array) {
                if ((bool)renderer) {
                    renderer.enabled = true;
                }
            }
        }

        void DisableRenderers() {
            disabledRenderers = GetComponentsInChildren<Renderer>();
            Renderer[] array = disabledRenderers;

            foreach (Renderer renderer in array) {
                renderer.enabled = false;
            }
        }
    }
}