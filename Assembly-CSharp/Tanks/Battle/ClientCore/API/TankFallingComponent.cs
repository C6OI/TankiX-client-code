using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class TankFallingComponent : Component {
        public int PreviousTrackContactsCount { get; set; }

        public int PreviousCollisionContactsCount { get; set; }

        public bool IsGrounded { get; set; }

        public Vector3 PreviousVelocity { get; set; }
    }
}