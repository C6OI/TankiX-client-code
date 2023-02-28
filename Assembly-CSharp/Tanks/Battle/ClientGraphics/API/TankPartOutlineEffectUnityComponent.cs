using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class TankPartOutlineEffectUnityComponent : BehaviourComponent {
        const string ALPHA_NAME = "_Alpha";

        [SerializeField] GameObject outlineEffectGameObject;

        int alphaPropertyId;

        MeshRenderer outlineMeshRenderer;

        public GameObject OutlineEffectGameObject {
            get => outlineEffectGameObject;
            set => outlineEffectGameObject = value;
        }

        public Material MaterialForTankPart { get; private set; }

        void Awake() {
            Mesh mesh = outlineEffectGameObject.GetComponent<MeshFilter>().mesh;
            mesh.bounds = new Bounds(mesh.bounds.center, mesh.bounds.size * 1000f);
            enabled = false;
            outlineEffectGameObject.SetActive(false);
        }

        void Update() {
            if (TankOutlineMapEffectComponent.IS_OUTLINE_EFFECT_RUNNING) {
                if (!outlineEffectGameObject.activeSelf) {
                    outlineEffectGameObject.SetActive(true);
                }
            } else if (outlineEffectGameObject.activeSelf) {
                outlineEffectGameObject.SetActive(false);
            }
        }

        public Material InitTankPartForOutlineEffect(Material materialForTankPart = null) {
            outlineMeshRenderer = outlineEffectGameObject.GetComponent<MeshRenderer>();
            outlineMeshRenderer.enabled = false;
            int num = outlineMeshRenderer.materials.Length;
            Material[] array = new Material[num];
            alphaPropertyId = Shader.PropertyToID("_Alpha");
            materialForTankPart = !(materialForTankPart == null) ? materialForTankPart : Instantiate(outlineMeshRenderer.materials[0]);

            for (int i = 0; i < num; i++) {
                array[i] = materialForTankPart;
            }

            outlineMeshRenderer.materials = array;
            MaterialForTankPart = materialForTankPart;
            materialForTankPart.SetFloat(alphaPropertyId, 1f);
            enabled = true;
            return materialForTankPart;
        }

        public void UpdateTankPartOutlineEffectTransparency(float alpha) {
            MaterialForTankPart.SetFloat(alphaPropertyId, alpha);
        }

        public void SwitchOutlineRenderer(bool enableRenderer) {
            outlineMeshRenderer.enabled = enableRenderer;
        }
    }
}