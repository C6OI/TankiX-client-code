using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")]
    public class ScreenSpaceAmbientOcclusion : MonoBehaviour {
        public enum SSAOSamples {
            Low = 0,
            Medium = 1,
            High = 2
        }

        public float m_Radius = 0.4f;

        public SSAOSamples m_SampleCount = SSAOSamples.Medium;

        public float m_OcclusionIntensity = 1.5f;

        public int m_Blur = 2;

        public int m_Downsampling = 2;

        public float m_OcclusionAttenuation = 1f;

        public float m_MinZ = 0.01f;

        public Shader m_SSAOShader;

        public Texture2D m_RandomTexture;

        Material m_SSAOMaterial;

        bool m_Supported;

        void Start() {
            if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth)) {
                m_Supported = false;
                enabled = false;
                return;
            }

            CreateMaterials();

            if (!m_SSAOMaterial || m_SSAOMaterial.passCount != 5) {
                m_Supported = false;
                enabled = false;
            } else {
                m_Supported = true;
            }
        }

        void OnEnable() {
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        void OnDisable() {
            DestroyMaterial(m_SSAOMaterial);
        }

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (!m_Supported || !m_SSAOShader.isSupported) {
                enabled = false;
                return;
            }

            CreateMaterials();
            m_Downsampling = Mathf.Clamp(m_Downsampling, 1, 6);
            m_Radius = Mathf.Clamp(m_Radius, 0.05f, 1f);
            m_MinZ = Mathf.Clamp(m_MinZ, 1E-05f, 0.5f);
            m_OcclusionIntensity = Mathf.Clamp(m_OcclusionIntensity, 0.5f, 4f);
            m_OcclusionAttenuation = Mathf.Clamp(m_OcclusionAttenuation, 0.2f, 2f);
            m_Blur = Mathf.Clamp(m_Blur, 0, 4);
            RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / m_Downsampling, source.height / m_Downsampling, 0);
            float fieldOfView = GetComponent<Camera>().fieldOfView;
            float farClipPlane = GetComponent<Camera>().farClipPlane;
            float num = Mathf.Tan(fieldOfView * ((float)Math.PI / 180f) * 0.5f) * farClipPlane;
            float x = num * GetComponent<Camera>().aspect;
            m_SSAOMaterial.SetVector("_FarCorner", new Vector3(x, num, farClipPlane));
            int num2;
            int num3;

            if ((bool)m_RandomTexture) {
                num2 = m_RandomTexture.width;
                num3 = m_RandomTexture.height;
            } else {
                num2 = 1;
                num3 = 1;
            }

            m_SSAOMaterial.SetVector("_NoiseScale", new Vector3(renderTexture.width / (float)num2, renderTexture.height / (float)num3, 0f));
            m_SSAOMaterial.SetVector("_Params", new Vector4(m_Radius, m_MinZ, 1f / m_OcclusionAttenuation, m_OcclusionIntensity));
            bool flag = m_Blur > 0;
            Graphics.Blit(!flag ? source : null, renderTexture, m_SSAOMaterial, (int)m_SampleCount);

            if (flag) {
                RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
                m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(m_Blur / (float)source.width, 0f, 0f, 0f));
                m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
                Graphics.Blit(null, temporary, m_SSAOMaterial, 3);
                RenderTexture.ReleaseTemporary(renderTexture);
                RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
                m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, m_Blur / (float)source.height, 0f, 0f));
                m_SSAOMaterial.SetTexture("_SSAO", temporary);
                Graphics.Blit(source, temporary2, m_SSAOMaterial, 3);
                RenderTexture.ReleaseTemporary(temporary);
                renderTexture = temporary2;
            }

            m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
            Graphics.Blit(source, destination, m_SSAOMaterial, 4);
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        static Material CreateMaterial(Shader shader) {
            if (!shader) {
                return null;
            }

            Material material = new(shader);
            material.hideFlags = HideFlags.HideAndDontSave;
            return material;
        }

        static void DestroyMaterial(Material mat) {
            if ((bool)mat) {
                DestroyImmediate(mat);
                mat = null;
            }
        }

        void CreateMaterials() {
            if (!m_SSAOMaterial && m_SSAOShader.isSupported) {
                m_SSAOMaterial = CreateMaterial(m_SSAOShader);
                m_SSAOMaterial.SetTexture("_RandomTexture", m_RandomTexture);
            }
        }
    }
}