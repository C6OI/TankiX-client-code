using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")]
    public class ContrastStretch : MonoBehaviour {
        public float adaptationSpeed = 0.02f;

        public float limitMinimum = 0.2f;

        public float limitMaximum = 0.6f;

        public Shader shaderLum;

        public Shader shaderReduce;

        public Shader shaderAdapt;

        public Shader shaderApply;

        readonly RenderTexture[] adaptRenderTex = new RenderTexture[2];

        int curAdaptIndex;

        Material m_materialAdapt;

        Material m_materialApply;

        Material m_materialLum;

        Material m_materialReduce;

        protected Material materialLum {
            get {
                if (m_materialLum == null) {
                    m_materialLum = new Material(shaderLum);
                    m_materialLum.hideFlags = HideFlags.HideAndDontSave;
                }

                return m_materialLum;
            }
        }

        protected Material materialReduce {
            get {
                if (m_materialReduce == null) {
                    m_materialReduce = new Material(shaderReduce);
                    m_materialReduce.hideFlags = HideFlags.HideAndDontSave;
                }

                return m_materialReduce;
            }
        }

        protected Material materialAdapt {
            get {
                if (m_materialAdapt == null) {
                    m_materialAdapt = new Material(shaderAdapt);
                    m_materialAdapt.hideFlags = HideFlags.HideAndDontSave;
                }

                return m_materialAdapt;
            }
        }

        protected Material materialApply {
            get {
                if (m_materialApply == null) {
                    m_materialApply = new Material(shaderApply);
                    m_materialApply.hideFlags = HideFlags.HideAndDontSave;
                }

                return m_materialApply;
            }
        }

        void Start() {
            if (!SystemInfo.supportsImageEffects) {
                enabled = false;
            } else if (!shaderAdapt.isSupported || !shaderApply.isSupported || !shaderLum.isSupported || !shaderReduce.isSupported) {
                enabled = false;
            }
        }

        void OnEnable() {
            for (int i = 0; i < 2; i++) {
                if (!adaptRenderTex[i]) {
                    adaptRenderTex[i] = new RenderTexture(1, 1, 0);
                    adaptRenderTex[i].hideFlags = HideFlags.HideAndDontSave;
                }
            }
        }

        void OnDisable() {
            for (int i = 0; i < 2; i++) {
                DestroyImmediate(adaptRenderTex[i]);
                adaptRenderTex[i] = null;
            }

            if ((bool)m_materialLum) {
                DestroyImmediate(m_materialLum);
            }

            if ((bool)m_materialReduce) {
                DestroyImmediate(m_materialReduce);
            }

            if ((bool)m_materialAdapt) {
                DestroyImmediate(m_materialAdapt);
            }

            if ((bool)m_materialApply) {
                DestroyImmediate(m_materialApply);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, renderTexture, materialLum);

            while (renderTexture.width > 1 || renderTexture.height > 1) {
                int num = renderTexture.width / 2;

                if (num < 1) {
                    num = 1;
                }

                int num2 = renderTexture.height / 2;

                if (num2 < 1) {
                    num2 = 1;
                }

                RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
                Graphics.Blit(renderTexture, temporary, materialReduce);
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = temporary;
            }

            CalculateAdaptation(renderTexture);
            materialApply.SetTexture("_AdaptTex", adaptRenderTex[curAdaptIndex]);
            Graphics.Blit(source, destination, materialApply);
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        void CalculateAdaptation(Texture curTexture) {
            int num = curAdaptIndex;
            curAdaptIndex = (curAdaptIndex + 1) % 2;
            float value = 1f - Mathf.Pow(1f - adaptationSpeed, 30f * Time.deltaTime);
            value = Mathf.Clamp(value, 0.01f, 1f);
            materialAdapt.SetTexture("_CurTex", curTexture);
            materialAdapt.SetVector("_AdaptParams", new Vector4(value, limitMinimum, limitMaximum, 0f));
            Graphics.SetRenderTarget(adaptRenderTex[curAdaptIndex]);
            GL.Clear(false, true, Color.black);
            Graphics.Blit(adaptRenderTex[num], adaptRenderTex[curAdaptIndex], materialAdapt);
        }
    }
}