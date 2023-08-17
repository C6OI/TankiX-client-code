using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GraffitiGarageItemsScreenComponent : MonoBehaviour, Component {
        [SerializeField] RawImage preview;

        public void SetPreview(Texture texture) => preview.texture = texture;
    }
}