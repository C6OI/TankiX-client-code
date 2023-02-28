using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EffectRendererGraphicsComponent : Component {
        public EffectRendererGraphicsComponent() { }

        public EffectRendererGraphicsComponent(Renderer renderer) => Renderer = renderer;

        public Renderer Renderer { get; set; }
    }
}