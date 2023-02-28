using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GraffitiPreviewComponent : BehaviourComponent {
        [SerializeField] RawImage preview;

        public void SetPreview(Texture texture) {
            preview.texture = texture;
            preview.gameObject.SetActive(true);
        }

        public void ResetPreview() {
            preview.gameObject.SetActive(false);
        }
    }
}