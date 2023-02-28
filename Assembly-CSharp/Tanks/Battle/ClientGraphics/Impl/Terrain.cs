using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class Terrain {
        public Terrain(List<GameObject> terrainObjects) {
            foreach (GameObject terrainObject in terrainObjects) {
                CollectMeshParts(terrainObject);
            }

            Bounds = CalculateBounds(MeshColliders);
        }

        public List<MeshCollider> MeshColliders { get; } = new();

        public List<MeshRenderer> MeshRenderers { get; } = new();

        public Bounds Bounds { get; }

        void CollectMeshParts(GameObject terrainObject) {
            MeshCollider[] componentsInChildren = terrainObject.GetComponentsInChildren<MeshCollider>();
            MeshCollider[] array = componentsInChildren;

            foreach (MeshCollider meshCollider in array) {
                if (meshCollider != null && meshCollider.enabled) {
                    MeshColliders.Add(meshCollider);
                }
            }

            MeshRenderer component = terrainObject.GetComponent<MeshRenderer>();

            if (component != null && component.enabled) {
                MeshRenderers.Add(component);
            }

            Transform transform = terrainObject.transform;

            for (int j = 0; j < transform.childCount; j++) {
                CollectMeshParts(transform.GetChild(j).gameObject);
            }
        }

        public static Bounds CalculateBounds(List<MeshCollider> meshColliders) {
            if (meshColliders.Count == 0) {
                return default;
            }

            Bounds result = meshColliders[0].bounds;

            for (int i = 1; i < meshColliders.Count; i++) {
                result.Encapsulate(meshColliders[i].bounds);
            }

            return result;
        }
    }
}