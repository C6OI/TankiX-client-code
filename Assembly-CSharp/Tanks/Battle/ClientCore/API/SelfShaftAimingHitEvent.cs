using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(8070042425022831807L)]
    public class SelfShaftAimingHitEvent : SelfHitEvent {
        public SelfShaftAimingHitEvent() { }

        public SelfShaftAimingHitEvent(List<HitTarget> targets, StaticHit staticHit)
            : base(targets, staticHit) { }

        public float HitPower { get; set; }
    }
}