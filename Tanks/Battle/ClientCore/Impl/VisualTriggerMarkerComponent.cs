using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class VisualTriggerMarkerComponent : MonoBehaviour {
        [SerializeField] MeshCollider visualTriggerMeshCollider;

        public MeshCollider VisualTriggerMeshCollider => visualTriggerMeshCollider;
    }
}