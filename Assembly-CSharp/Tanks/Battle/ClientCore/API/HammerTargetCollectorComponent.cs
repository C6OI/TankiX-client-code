using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    public class HammerTargetCollectorComponent : TargetCollectorComponent {
        public HammerTargetCollectorComponent(TargetCollector targetCollector, TargetValidator targetValidator)
            : base(targetCollector, targetValidator) { }
    }
}