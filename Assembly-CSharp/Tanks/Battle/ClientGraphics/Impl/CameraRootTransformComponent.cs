using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraRootTransformComponent : Component {
        public CameraRootTransformComponent(Transform root) => Root = root;

        public Transform Root { get; set; }
    }
}