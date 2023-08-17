using Platform.Library.ClientResources.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class WeaponDetachColliderComponent : WarmableResourceBehaviour, Component {
        [SerializeField] MeshCollider collider;

        [SerializeField] Rigidbody rigidbody;

        public MeshCollider Collider => collider;

        public Rigidbody Rigidbody => rigidbody;

        void Awake() => rigidbody.detectCollisions = false;

        public override void WarmUp() => collider.enabled = true;
    }
}