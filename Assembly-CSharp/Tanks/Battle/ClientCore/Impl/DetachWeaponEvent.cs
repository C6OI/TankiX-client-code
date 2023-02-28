using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1447917521601L)]
    public class DetachWeaponEvent : Event {
        public DetachWeaponEvent() { }

        public DetachWeaponEvent(Vector3 velocity, Vector3 angularVelocity) {
            Velocity = velocity;
            AngularVelocity = angularVelocity;
        }

        public Vector3 Velocity { get; set; }

        public Vector3 AngularVelocity { get; set; }
    }
}