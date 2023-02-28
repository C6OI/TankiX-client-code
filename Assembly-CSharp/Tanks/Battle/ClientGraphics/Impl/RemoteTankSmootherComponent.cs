using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RemoteTankSmootherComponent : Component {
        public Vector3 prevVisualPosition = Vector3.zero;

        public Quaternion prevVisualRotation = Quaternion.identity;
        public float smoothingCoeff = 20f;
    }
}