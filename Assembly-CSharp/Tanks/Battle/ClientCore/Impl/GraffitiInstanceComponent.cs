using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class GraffitiInstanceComponent : Component {
        public GraffitiInstanceComponent() { }

        public GraffitiInstanceComponent(GameObject graffitiInstance) => GraffitiInstance = graffitiInstance;

        public GameObject GraffitiInstance { get; set; }

        public GameObject GraffitiDecalObject { get; set; }

        public Renderer EmitRenderer { get; set; }
    }
}