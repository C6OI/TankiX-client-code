using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BezierPositionComponent : Component {
        public BezierPositionComponent() => BezierPosition = new BezierPosition();

        public BezierPosition BezierPosition { get; set; }
    }
}