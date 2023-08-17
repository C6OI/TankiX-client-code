using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class TankCollidersComponent : Component {
        public BoxCollider BoundsCollider { get; set; }

        public Collider TankToTankCollider { get; set; }

        public List<GameObject> TargetingColliders { get; set; }

        public List<GameObject> VisualTriggerColliders { get; set; }

        public List<Collider> TankToStaticColliders { get; set; }

        public Vector3 Extends { get; set; }
    }
}