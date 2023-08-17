using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BulletConfigComponent : Component {
        public float FullDistance { get; set; }

        public float RadialRaysCount { get; set; }
    }
}