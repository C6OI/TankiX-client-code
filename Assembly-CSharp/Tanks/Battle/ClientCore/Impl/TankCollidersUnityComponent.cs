using System;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankCollidersUnityComponent : MonoBehaviour, Component {
        public static readonly string BOUNDS_COLLIDER_NAME = "bounds";

        public static readonly string TANK_TO_STATIC_COLLIDER_NAME = "tank_to_static";

        public static readonly string TANK_TO_STATIC_TOP_COLLIDER_NAME = "top";

        public static readonly string TANK_TO_TANK_COLLIDER_NAME = "tank_to_tank";

        public static readonly string TARGETING_COLLIDER_NAME = "target";

        public static readonly string FRICTION_COLLIDERS_ROOT_NAME = "friction";

        public float a = 0.82f;

        public float inclineSubstraction = 0.1f;

        public PhysicMaterial lowFrictionMaterial;

        public PhysicMaterial highFrictionMaterial;

        void Awake() {
            GetBoundsCollider().enabled = false;
        }

        public Vector3 GetBoundsCenterGlobal() {
            BoxCollider boundsCollider = GetBoundsCollider();
            return boundsCollider.transform.TransformPoint(boundsCollider.center);
        }

        public BoxCollider GetBoundsCollider() => FindChildCollider(BOUNDS_COLLIDER_NAME).GetComponent<BoxCollider>();

        public Collider GetTankToTankCollider() => FindChildCollider(TANK_TO_TANK_COLLIDER_NAME).GetComponent<Collider>();

        public Collider GetTargetingCollider() => FindChildCollider(TARGETING_COLLIDER_NAME).GetComponent<Collider>();

        public Collider GetTankToStaticTopCollider() => FindChildCollider(TANK_TO_STATIC_TOP_COLLIDER_NAME).GetComponent<Collider>();

        GameObject FindChildCollider(string childName) {
            Collider[] componentsInChildren = transform.GetComponentsInChildren<Collider>(true);

            foreach (Collider collider in componentsInChildren) {
                if (collider.name.Equals(childName, StringComparison.OrdinalIgnoreCase)) {
                    return collider.gameObject;
                }
            }

            throw new ColliderNotFoundException(this, childName);
        }
    }
}