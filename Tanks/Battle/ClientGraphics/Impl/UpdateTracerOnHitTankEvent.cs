using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateTracerOnHitTankEvent : Event {
        public HitTarget TankHit { get; set; }

        public void Init(HitTarget tankHit) => TankHit = tankHit;
    }
}