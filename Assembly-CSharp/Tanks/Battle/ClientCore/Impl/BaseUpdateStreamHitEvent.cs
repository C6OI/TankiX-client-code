using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BaseUpdateStreamHitEvent : Event {
        public BaseUpdateStreamHitEvent() { }

        public BaseUpdateStreamHitEvent(HitTarget tankHit, StaticHit staticHit) {
            TankHit = tankHit;
            StaticHit = staticHit;
        }

        [ProtocolOptional] public HitTarget TankHit { get; set; }

        [ProtocolOptional] public StaticHit StaticHit { get; set; }
    }
}