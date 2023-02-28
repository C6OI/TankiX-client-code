using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankPartPaintInstanceComponent : Component {
        public TankPartPaintInstanceComponent() { }

        public TankPartPaintInstanceComponent(GameObject paintInstance) => PaintInstance = paintInstance;

        public GameObject PaintInstance { get; set; }
    }
}