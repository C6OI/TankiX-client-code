using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.API {
    public abstract class BulletHitEvent : Event {
        public Vector3 Position { get; set; }
    }
}