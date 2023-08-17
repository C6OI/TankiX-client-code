using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class TransformTimeSmoothingComponent : Component {
        public TransformTimeSmoothingComponent(Transform transform) => Transform = transform;

        public TransformTimeSmoothingComponent() { }

        public bool UseCorrectionByFrameLeader { get; set; }

        public Transform Transform { get; set; }
    }
}