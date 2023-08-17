using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class WeaponBlockedComponent : Component {
        public Vector3 BlockPoint { get; set; }

        public Vector3 BlockNormal { get; set; }

        public GameObject BlockGameObject { get; set; }
    }
}