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
            MeshCollider component = terrainObject.GetComponent<MeshCollider>();

            if (component != null && component.enabled) {
                MeshColliders.Add(component);
            }

            MeshRenderer component2 = terrainObject.GetComponent<MeshRenderer>();

            if (component2 != null && component2.enabled) {
                MeshRenderers.Add(component2);
            }

            Transform transform = terrainObject.transform;

            for (int i = 0; i < transform.childCount; i++) {
                CollectMeshParts(transform.GetChild(i).gameObject);
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