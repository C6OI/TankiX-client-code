using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ExplosionEvent : Event {
        public HitTarget Target;

        public GameObject Asset { get; set; }

        public Vector3 ExplosionOffset { get; set; }

        public Vector3 HitDirection { get; set; }

        public float Duration { get; set; }
    }
}