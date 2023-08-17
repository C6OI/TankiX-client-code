using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class BulletTargetingComponent : Component {
        public float Radius { get; set; }

        public float RadialRaysCount { get; set; }
    }
}