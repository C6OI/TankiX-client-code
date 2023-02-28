using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class InitMinePlacingTransformEvent : Event {
        public InitMinePlacingTransformEvent(Vector3 position) => Position = position;

        public Vector3 Position { get; set; }
    }
}