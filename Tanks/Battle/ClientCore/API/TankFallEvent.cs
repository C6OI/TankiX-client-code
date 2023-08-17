using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.API {
    public class TankFallEvent : Event {
        public float FallingPower { get; set; }

        public TankFallingType FallingType { get; set; }

        public Transform FallingTransform { get; set; }
    }
}