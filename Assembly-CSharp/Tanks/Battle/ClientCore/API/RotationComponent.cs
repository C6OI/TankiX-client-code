using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-1853333282151870933L)]
    public class RotationComponent : Component {
        public RotationComponent() { }

        public RotationComponent(Vector3 rotationEuler) => RotationEuler = rotationEuler;

        public Vector3 RotationEuler { get; set; }
    }
}