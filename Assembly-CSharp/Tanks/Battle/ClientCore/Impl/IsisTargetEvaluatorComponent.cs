using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IsisTargetEvaluatorComponent : Component {
        public int? LastDirectionIndex { get; set; }
    }
}