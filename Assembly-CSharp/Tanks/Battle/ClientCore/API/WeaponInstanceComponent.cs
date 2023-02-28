using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class WeaponInstanceComponent : Component {
        public WeaponInstanceComponent() { }

        public WeaponInstanceComponent(GameObject weaponInstance) => WeaponInstance = weaponInstance;

        public GameObject WeaponInstance { get; set; }
    }
}