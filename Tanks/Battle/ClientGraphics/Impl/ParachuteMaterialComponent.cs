using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ParachuteMaterialComponent : Component {
        public ParachuteMaterialComponent(Material material) => Material = material;

        public Material Material { get; set; }
    }
}