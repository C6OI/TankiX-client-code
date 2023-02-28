using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Blur/Blur")]
    public class Blur : MonoBehaviour {
        static Material m_Material;
        public int iterations = 3;

        public float blurSpread = 0.6f;

        public Shader blurShader;

        protected Material material {
            get {
                if (m_Material == null) {
                    m_Material = new Material(blurShader);
                    m_Material.hideFlags = HideFlags.DontSave;
                }

                return m_Material;
            }
        }

        protected void Start() {
            if (!SystemInfo.supportsImageEffects) {
                enabled = false;
            } else if (!blurShader || !material.shader.isSupported) {
                enabled = false;
            }
        }

        protected void OnDisable() {
            if ((bool)m_Material) {
                DestroyImmediate(m_Material);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            int width = source.width / 4;
            int height = source.height / 4;
            RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
            DownSample4x(source, renderTexture);

            for (int i = 0; i < iterations; i++) {
                RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
                FourTapCone(renderTexture, temporary, i);
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = temporary;
            }

            Graphics.Blit(renderTexture, destination);
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration) {
            float num = 0.5f + iteration * blurSpread;
            Graphics.BlitMultiTap(source, dest, material, new Vector2(0f - num, 0f - num), new Vector2(0f - num, num), new Vector2(num, num), new Vector2(num, 0f - num));
        }

        void DownSample4x(RenderTexture source, RenderTexture dest) {
            float num = 1f;
            Graphics.BlitMultiTap(source, dest, material, new Vector2(0f - num, 0f - num), new Vector2(0f - num, num), new Vector2(num, num), new Vector2(num, 0f - num));
        }
    }
}