using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplatePositionComponent : Component {
        public Vector3 previousPosition;
        public float sqrDistance;
    }
}