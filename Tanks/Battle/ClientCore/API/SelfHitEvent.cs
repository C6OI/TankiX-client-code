using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(8814758840778124785L)]
    public class SelfHitEvent : HitEvent {
        public SelfHitEvent() { }

        public SelfHitEvent(List<HitTarget> targets, StaticHit staticHit)
            : base(targets, staticHit) { }

        public override string ToString() => string.Format("Targets: {0}, StaticHit: {1}",
            string.Join(",", Targets.Select(t => t.ToString()).ToArray()),
            StaticHit);
    }
}