using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class BaseRendererComponent : Component {
        public Renderer Renderer { get; set; }

        public Mesh Mesh { get; set; }
    }
}