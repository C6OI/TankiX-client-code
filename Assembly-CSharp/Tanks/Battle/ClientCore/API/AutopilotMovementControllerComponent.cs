using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1508219865424L)]
    public class AutopilotMovementControllerComponent : Component {
        public bool Moving { get; set; }

        public bool MoveToTarget { get; set; }

        [ProtocolOptional] public Entity Target { get; set; }

        public Vector3 Destination { get; set; }
    }
}