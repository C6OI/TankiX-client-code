using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(3441234123559L)]
    [Shared]
    public class DroneMoveConfigComponent : Component {
        public float Acceleration { get; set; }

        public Vector3 SpawnPosition { get; set; }

        public Vector3 FlyPosition { get; set; }

        public float RotationSpeed { get; set; }

        public float MoveSpeed { get; set; }
    }
}