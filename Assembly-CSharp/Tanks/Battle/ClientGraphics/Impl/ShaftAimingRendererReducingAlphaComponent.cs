using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingRendererReducingAlphaComponent : Component {
        public ShaftAimingRendererReducingAlphaComponent() { }

        public ShaftAimingRendererReducingAlphaComponent(float initialAlpha) => InitialAlpha = initialAlpha;

        public float InitialAlpha { get; set; }
    }
}