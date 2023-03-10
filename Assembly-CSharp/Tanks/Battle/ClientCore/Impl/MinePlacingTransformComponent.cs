using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class MinePlacingTransformComponent : Component {
        public RaycastHit PlacingData { get; set; }

        public bool HasPlacingTransform { get; set; }
    }
}