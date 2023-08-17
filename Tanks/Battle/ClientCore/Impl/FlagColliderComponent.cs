using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class FlagColliderComponent : Component {
        public FlagColliderComponent() { }

        public FlagColliderComponent(BoxCollider boxCollider) => this.boxCollider = boxCollider;

        public BoxCollider boxCollider { get; set; }
    }
}