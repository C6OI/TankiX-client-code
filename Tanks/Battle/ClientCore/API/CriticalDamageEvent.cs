using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-4247034853035810941L)]
    [Shared]
    public class CriticalDamageEvent : Event {
        public Entity Target { get; set; }

        public Vector3 LocalPosition { get; set; }
    }
}