using System.Collections;
using UnityEngine;

namespace MeshBrush {
    public class MeshBrushParent : MonoBehaviour {
        Renderer curRenderer;

        ArrayList elements;

        MeshFilter filter;

        CombineUtility.MeshInstance instance;

        CombineUtility.MeshInstance[] instances;

        Material[] materials;

        Hashtable materialToMesh;
        Transform[] meshes;

        Component[] meshFilters;

        Matrix4x4 myTransform;

        ArrayList objects;

        void Start() {
            Destroy(this);
        }
    }
}