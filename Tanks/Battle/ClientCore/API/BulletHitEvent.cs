using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.API {
    public class BulletHitEvent : Event {
        public BulletHitEvent() { }

        public BulletHitEvent(Vector3 position) => Position = position;

        public Vector3 Position { get; set; }
    }
}