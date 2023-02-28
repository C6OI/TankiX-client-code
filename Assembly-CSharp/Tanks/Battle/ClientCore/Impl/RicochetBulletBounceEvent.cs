using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class RicochetBulletBounceEvent : Event {
        public RicochetBulletBounceEvent() { }

        public RicochetBulletBounceEvent(Vector3 worldSpaceBouncePosition) => WorldSpaceBouncePosition = worldSpaceBouncePosition;

        public Vector3 WorldSpaceBouncePosition { get; set; }
    }
}