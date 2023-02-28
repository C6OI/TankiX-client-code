using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class StartMaterialsComponent : Component {
        public Material[] Materials { get; set; }
    }
}