using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class PaintInstanceComponent : Component {
        public PaintInstanceComponent() { }

        public PaintInstanceComponent(GameObject paintInstance) => PaintInstance = paintInstance;

        public GameObject PaintInstance { get; set; }
    }
}