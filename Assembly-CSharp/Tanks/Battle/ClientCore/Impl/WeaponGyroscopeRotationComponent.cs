using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponGyroscopeRotationComponent : Component {
        public float weaponTurnCoeff = 1f;

        public float GyroscopePower { get; set; }

        public Vector3 ForwardDir { get; set; }

        public Vector3 UpDir { get; set; }

        public float WeaponTurnCoeff {
            get => weaponTurnCoeff;
            set => weaponTurnCoeff = value;
        }

        public float DeltaAngleOfHullRotation { get; set; }
    }
}