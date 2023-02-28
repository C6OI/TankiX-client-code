using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class DroneOwnerComponent : Component {
        public Entity Incarnation { get; set; }

        public Rigidbody Rigidbody { get; set; }
    }
}