using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class ShaftAimingTargetPointContainerComponent : Component {
        public Vector3 Point { get; set; }

        public bool IsInsideTankPart { get; set; }

        public float PrevVerticalAngle { get; set; }
    }
}