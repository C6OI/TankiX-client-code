using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateWeaponStreamHitGraphicsByTargetTankEvent : Event {
        public ParticleSystem HitTargetParticleSystem { get; set; }

        public Light HitTargetLight { get; set; }

        public HitTarget TankHit { get; set; }

        public float HitOffset { get; set; }
    }
}