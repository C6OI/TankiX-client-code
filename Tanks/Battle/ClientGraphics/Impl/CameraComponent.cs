using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraComponent : Component {
        public CameraComponent() { }

        public CameraComponent(Camera camera) => Camera = camera;

        public Camera Camera { get; set; }
    }
}