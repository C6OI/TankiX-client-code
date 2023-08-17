using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636100801609006236L)]
    public class GraffitiDecalComponent : Component {
        public GraffitiDecalComponent() { }

        public GraffitiDecalComponent(Vector3 sprayPosition, Vector3 sprayDirection, Vector3 sprayUpDirection) {
            SprayPosition = sprayPosition;
            SprayDirection = sprayDirection;
            SprayUpDirection = sprayUpDirection;
        }

        public Vector3 SprayPosition { get; set; }

        public Vector3 SprayDirection { get; set; }

        public Vector3 SprayUpDirection { get; set; }
    }
}