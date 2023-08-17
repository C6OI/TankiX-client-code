using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class TargetCollectorComponent : Component {
        public TargetCollectorComponent() => Mask = LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS;

        public int Mask { get; set; }
    }
}