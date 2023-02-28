using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(4605414188335188027L)]
    public class PositionComponent : Component {
        public PositionComponent() { }

        public PositionComponent(Vector3 position) => Position = position;

        public Vector3 Position { get; set; }
    }
}