using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SeparateParachuteComponent : Component {
        public float parachuteFoldingScaleByY { get; set; }

        public float parachuteFoldingScaleByXZ { get; set; }
    }
}