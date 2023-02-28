using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class TankPartIntersectionWithCameraData {
        public readonly Collider collider;

        public readonly Entity entity;

        public TankPartIntersectionWithCameraData(Collider collider, Entity entity) {
            this.collider = collider;
            this.entity = entity;
        }
    }
}