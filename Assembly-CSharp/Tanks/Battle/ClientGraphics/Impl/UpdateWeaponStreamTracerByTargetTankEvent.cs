using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateWeaponStreamTracerByTargetTankEvent : Event {
        public GameObject WeaponStreamTracerInstance { get; set; }

        public WeaponStreamTracerBehaviour WeaponStreamTracerBehaviour { get; set; }

        public HitTarget Hit { get; set; }
    }
}