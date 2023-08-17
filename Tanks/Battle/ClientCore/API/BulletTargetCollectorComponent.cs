using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class BulletTargetCollectorComponent : Component {
        public BulletTargetCollectorComponent() { }

        public BulletTargetCollectorComponent(bool useRaycastExclusion) => UseRaycastExclusion = useRaycastExclusion;

        public bool UseRaycastExclusion { get; set; }
    }
}