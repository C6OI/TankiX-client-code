using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class MaterialArrayComponent : Component {
        public MaterialArrayComponent(Material[] materials) => Materials = materials;

        public Material[] Materials { get; set; }
    }
}