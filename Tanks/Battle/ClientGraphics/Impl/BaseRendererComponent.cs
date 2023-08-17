using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BaseRendererComponent : Component {
        public Renderer Renderer { get; set; }

        public Mesh Mesh { get; set; }
    }
}