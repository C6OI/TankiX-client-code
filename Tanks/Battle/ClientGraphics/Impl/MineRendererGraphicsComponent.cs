using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineRendererGraphicsComponent : Component {
        public MineRendererGraphicsComponent() { }

        public MineRendererGraphicsComponent(Renderer renderer) => Renderer = renderer;

        public Renderer Renderer { get; set; }
    }
}