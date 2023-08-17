using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class WeaponBoundsComponent : Component {
        public WeaponBoundsComponent() { }

        public WeaponBoundsComponent(Bounds weaponBounds) => WeaponBounds = weaponBounds;

        public Bounds WeaponBounds { get; set; }
    }
}