using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.API {
    public class CriticalEffectEvent : Event {
        public GameObject EffectPrefab { get; set; }

        public Vector3 LocalPosition { get; set; }
    }
}