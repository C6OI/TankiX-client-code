using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class SendHitToServerEvent : Event {
        public SendHitToServerEvent() => Targets = new List<HitTarget>();

        public SendHitToServerEvent(TargetingData targetingData, List<HitTarget> targets, StaticHit staticHit) {
            TargetingData = targetingData;
            Targets = targets;
            StaticHit = staticHit;
        }

        public SendHitToServerEvent(TargetingData targetingData) => TargetingData = targetingData;

        public TargetingData TargetingData { get; set; }

        public List<HitTarget> Targets { get; set; }

        public StaticHit StaticHit { get; set; }
    }
}