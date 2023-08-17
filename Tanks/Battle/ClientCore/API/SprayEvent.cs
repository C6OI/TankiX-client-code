using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.API {
    public class SprayEvent : Event {
        public SprayEvent() { }

        public SprayEvent(Vector3 position) => Position = position;

        public Vector3 Position { get; set; }

        public override string ToString() => string.Format("Position: {0}", Position);
    }
}