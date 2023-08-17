using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateVulcanTracerByTargetTankEvent : Event {
        public GameObject VulcanTracerInstance { get; set; }

        public VulcanTracerBehaviour VulcanTracerBehaviour { get; set; }

        public HitTarget Hit { get; set; }
    }
}