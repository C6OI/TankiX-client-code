using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SerialVersionUID(635824352101335226L)]
    public class CartridgeCaseContainerComponent : Component {
        public GameObject container;

        public CartridgeCaseContainerComponent() { }

        public CartridgeCaseContainerComponent(GameObject container) => this.container = container;
    }
}