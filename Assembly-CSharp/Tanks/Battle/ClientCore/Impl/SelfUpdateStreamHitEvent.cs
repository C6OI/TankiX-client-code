using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1430210549752L)]
    public class SelfUpdateStreamHitEvent : BaseUpdateStreamHitEvent {
        public SelfUpdateStreamHitEvent() { }

        public SelfUpdateStreamHitEvent(HitTarget tankHit, StaticHit staticHit)
            : base(tankHit, staticHit) { }
    }
}