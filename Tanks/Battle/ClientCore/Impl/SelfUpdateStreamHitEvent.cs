using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1430210549752L)]
    [Shared]
    public class SelfUpdateStreamHitEvent : BaseUpdateStreamHitEvent {
        public SelfUpdateStreamHitEvent() { }

        public SelfUpdateStreamHitEvent(HitTarget tankHit, StaticHit staticHit)
            : base(tankHit, staticHit) { }
    }
}