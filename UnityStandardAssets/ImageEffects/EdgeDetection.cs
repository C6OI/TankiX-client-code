using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class EdgeDetection : PostEffectsBase {
        public enum EdgeDetectMode {
            TriangleDepthNormals = 0,
            RobertsCrossDepthNormals = 1,
            SobelDepth = 2,
            SobelDepthThin = 3,
            TriangleLuminance = 4
        }

        public EdgeDetectMode mode = EdgeDetectMode.SobelDepthThin;

        public float sensitivityDepth = 1f;

        public float sensitivityNormals = 1f;

        public float lumThreshold = 0.2f;

        public float edgeExp = 1f;

        public float sampleDist = 1f;

        public float edgesOnly;

        public Color edgesOnlyBgColor = Color.white;

        public Shader edgeDetectShader;

        Material edgeDetectMaterial;

        EdgeDetectMode oldMode = EdgeDetectMode.SobelDepthThin;

        new void Start() => oldMode = mode;

        void OnEnable() => SetCameraFlag();

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (!CheckResources()) {
                Graphics.Blit(source, destination);
                return;
            }

            Vector2 vector = new(sensitivityDepth, sensitivityNormals);
            edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector.x, vector.y, 1f, vector.y));
            edgeDetectMaterial.SetFloat("_BgFade", edgesOnly);
            edgeDetectMaterial.SetFloat("_SampleDistance", sampleDist);
            edgeDetectMaterial.SetVector("_BgColor", edgesOnlyBgColor);
            edgeDetectMaterial.SetFloat("_Exponent", edgeExp);
            edgeDetectMaterial.SetFloat("_Threshold", lumThreshold);
            Graphics.Blit(source, destination, edgeDetectMaterial, (int)mode);
        }

        public override bool CheckResources() {
            CheckSupport(true);
            edgeDetectMaterial = CheckShaderAndCreateMaterial(edgeDetectShader, edgeDetectMaterial);

            if (mode != oldMode) {
                SetCameraFlag();
            }

            oldMode = mode;

            if (!isSupported) {
                ReportAutoDisable();
            }

            return isSupported;
        }

        void SetCameraFlag() {
            if (mode == EdgeDetectMode.SobelDepth || mode == EdgeDetectMode.SobelDepthThin) {
                GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
            } else if (mode == EdgeDetectMode.TriangleDepthNormals || mode == EdgeDetectMode.RobertsCrossDepthNormals) {
                GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
            }
        }
    }
}