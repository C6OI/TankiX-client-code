using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class BulletBuildEvent : Event {
        public BulletBuildEvent() { }

        public BulletBuildEvent(Vector3 direction) => Direction = direction;

        public Vector3 Direction { get; set; }
    }
}