using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.API {
    public class DynamicShadowsFromLightmappedObjectsController : MonoBehaviour {
        public void Awake() {
            MeshRenderer[] componentsInChildren = this.gameObject.GetComponentsInChildren<MeshRenderer>();
            GameObject shadowCastersRoot = CreateShadowCastersRoot();
            MeshRenderer[] array = componentsInChildren;

            foreach (MeshRenderer meshRenderer in array) {
                if (meshRenderer.lightmapIndex >= 0) {
                    GameObject gameObject = CreateShadowCaster(meshRenderer, shadowCastersRoot);
                    gameObject.AddComponent<MeshFilter>().sharedMesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;
                    MeshRenderer meshRenderer2 = gameObject.AddComponent<MeshRenderer>();
                    meshRenderer2.sharedMaterials = meshRenderer.sharedMaterials;
                    meshRenderer2.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    meshRenderer2.receiveShadows = false;
                    meshRenderer2.reflectionProbeUsage = ReflectionProbeUsage.Off;
                    meshRenderer2.lightProbeUsage = LightProbeUsage.Off;
                }
            }
        }

        static GameObject CreateShadowCaster(MeshRenderer meshRenderer, GameObject shadowCastersRoot) {
            GameObject gameObject = new(meshRenderer.name + "_ShadowCaster");
            gameObject.transform.SetParent(shadowCastersRoot.transform);
            gameObject.transform.position = meshRenderer.transform.position;
            gameObject.transform.rotation = meshRenderer.transform.rotation;
            gameObject.transform.localScale = meshRenderer.transform.lossyScale;
            return gameObject;
        }

        GameObject CreateShadowCastersRoot() {
            GameObject gameObject = new("DynamicShadowsCasters");
            gameObject.transform.SetParent(transform);
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;
            return gameObject;
        }
    }
}