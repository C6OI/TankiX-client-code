using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShadowCasterCreatorBehaviour : MonoBehaviour {
        void Awake() {
            HashSet<MeshRenderer> hashSet = new();
            GameObject[] array = GameObject.FindGameObjectsWithTag("CASTSHADOW");

            foreach (GameObject gameObject in array) {
                MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer item in componentsInChildren) {
                    hashSet.Add(item);
                }
            }

            foreach (MeshRenderer item2 in hashSet) {
                MeshFilter component = item2.GetComponent<MeshFilter>();

                if (item2.enabled && (bool)component) {
                    Mesh sharedMesh = component.sharedMesh;
                    GameObject gameObject2 = new("Shadow");
                    gameObject2.AddComponent<MeshFilter>().sharedMesh = sharedMesh;
                    MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
                    meshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    meshRenderer.sharedMaterials = item2.sharedMaterials;
                    gameObject2.transform.SetParent(item2.transform, false);
                }
            }
        }
    }
}