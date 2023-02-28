using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TransformTimeSmoothingDataComponent : Component {
        public Vector3 LastPosition { get; set; }

        public Quaternion LastRotation { get; set; }

        public float LerpFactor { get; set; }

        public float LastRotationDeltaAngle { get; set; }
    }
}