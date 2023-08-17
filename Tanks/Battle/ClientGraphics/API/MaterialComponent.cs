using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class MaterialComponent : Component {
        public MaterialComponent(Material material) => Material = material;

        public Material Material { get; set; }
    }
}