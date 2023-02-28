using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    [Serializable]
    public class GrassPrefabData {
        public GameObject grassPrefab;

        public float minScale = 1f;

        public float maxScale = 1f;

        public bool IsValid(out string errorReason) {
            if (grassPrefab == null) {
                errorReason = "GrassPrefab is null";
                return false;
            }

            if (grassPrefab.GetComponent<MeshFilter>() == null) {
                errorReason = "GrassPrefab hasn't component MeshFilter";
                return false;
            }

            if (grassPrefab.GetComponent<MeshFilter>().sharedMesh == null) {
                errorReason = "Property sharedMesh of GrassPrefab's component MeshFilter is null";
                return false;
            }

            if (grassPrefab.GetComponent<MeshRenderer>() == null) {
                errorReason = "GrassPrefab hasn't component MeshRenderer";
                return false;
            }

            if (grassPrefab.GetComponent<MeshRenderer>().sharedMaterial == null) {
                errorReason = "Property sharedMaterial of GrassPrefab's component MeshRenderer is null";
                return false;
            }

            if (minScale.Equals(0f)) {
                errorReason = "MinScale can't be zero";
                return false;
            }

            if (maxScale.Equals(0f)) {
                errorReason = "MaxScale can't be zero";
                return false;
            }

            if (minScale > maxScale) {
                errorReason = "MinScale can't be more than maxScale";
                return false;
            }

            errorReason = string.Empty;
            return true;
        }

        public override string ToString() => string.Format("[Prefab: {0}, minScale: {1}, maxScale: {2}]", !(grassPrefab == null) ? grassPrefab.name : "null", minScale, maxScale);
    }
}