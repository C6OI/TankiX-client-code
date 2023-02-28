using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(8960819779144518L)]
    public class SpatialGeometryComponent : Component {
        public SpatialGeometryComponent() { }

        public SpatialGeometryComponent(Vector3 postion, Vector3 rotation) {
            Position = postion;
            Rotation = rotation;
        }

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }
    }
}