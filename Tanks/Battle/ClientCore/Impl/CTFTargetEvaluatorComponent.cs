using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class CTFTargetEvaluatorComponent : Component {
        public CTFTargetEvaluatorComponent() { }

        public CTFTargetEvaluatorComponent(float flagCarrierPriorityBonus) =>
            FlagCarrierPriorityBonus = flagCarrierPriorityBonus;

        public float FlagCarrierPriorityBonus { get; set; }
    }
}