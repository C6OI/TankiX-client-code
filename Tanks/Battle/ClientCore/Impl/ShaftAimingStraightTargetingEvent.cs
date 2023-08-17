using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingStraightTargetingEvent : Event {
        public TargetingData TargetingData { get; set; }

        public Vector3 WorkingDirection { get; set; }
    }
}