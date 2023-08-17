using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankCollidersUnityComponent : MonoBehaviour, Component {
        public static readonly string BOUNDS_COLLIDER_NAME = "bounds";

        public static readonly string TANK_TO_STATIC_COLLIDER_NAME = "tank_to_static";

        public static readonly string TANK_TO_TANK_COLLIDER_NAME = "tank_to_tank";

        public static readonly string TARGETING_COLLIDER_NAME = "target";

        public static readonly string FRICTION_COLLIDERS_ROOT_NAME = "friction";

        public float a = 0.82f;

        public float inclineSubstraction = 0.1f;

        public PhysicMaterial lowFrictionMaterial;

        public PhysicMaterial highFrictionMaterial;

        void Awake() => GetBoundsCollider().enabled = false;

        public Vector3 GetBoundsCenterGlobal() {
            BoxCollider boundsCollider = GetBoundsCollider();
            return boundsCollider.transform.TransformPoint(boundsCollider.center);
        }

        public BoxCollider GetBoundsCollider() => FindChild(BOUNDS_COLLIDER_NAME).GetComponent<BoxCollider>();

        public Collider GetTankToTankCollider() => FindChild(TANK_TO_TANK_COLLIDER_NAME).GetComponent<Collider>();

        public Collider GetTargetingCollider() => FindChild(TARGETING_COLLIDER_NAME).GetComponent<Collider>();

        GameObject FindChild(string childName) {
            Transform transform = this.transform.Find(childName);

            if (transform == null) {
                throw new ColliderNotFoundException(this, childName);
            }

            return transform.gameObject;
        }
    }
}