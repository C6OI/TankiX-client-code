using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class ImpactEvent : Event {
        public Vector3 LocalHitPoint { get; set; }

        public Vector3 Force { get; set; }
    }
}