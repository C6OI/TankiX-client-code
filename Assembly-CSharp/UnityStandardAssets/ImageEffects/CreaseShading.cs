using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Edge Detection/Crease Shading")]
    internal class CreaseShading : PostEffectsBase {
        public float intensity = 0.5f;

        public int softness = 1;

        public float spread = 1f;

        public Shader blurShader;

        public Shader depthFetchShader;

        public Shader creaseApplyShader;

        Material blurMaterial;

        Material creaseApplyMaterial;

        Material depthFetchMaterial;

        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (!CheckResources()) {
                Graphics.Blit(source, destination);
                return;
            }

            int width = source.width;
            int height = source.height;
            float num = 1f * width / (1f * height);
            float num2 = 0.001953125f;
            RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
            RenderTexture renderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0);
            Graphics.Blit(source, temporary, depthFetchMaterial);
            Graphics.Blit(temporary, renderTexture);

            for (int i = 0; i < softness; i++) {
                RenderTexture temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
                blurMaterial.SetVector("offsets", new Vector4(0f, spread * num2, 0f, 0f));
                Graphics.Blit(renderTexture, temporary2, blurMaterial);
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = temporary2;
                temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
                blurMaterial.SetVector("offsets", new Vector4(spread * num2 / num, 0f, 0f, 0f));
                Graphics.Blit(renderTexture, temporary2, blurMaterial);
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = temporary2;
            }

            creaseApplyMaterial.SetTexture("_HrDepthTex", temporary);
            creaseApplyMaterial.SetTexture("_LrDepthTex", renderTexture);
            creaseApplyMaterial.SetFloat("intensity", intensity);
            Graphics.Blit(source, destination, creaseApplyMaterial);
            RenderTexture.ReleaseTemporary(temporary);
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        public override bool CheckResources() {
            CheckSupport(true);
            blurMaterial = CheckShaderAndCreateMaterial(blurShader, blurMaterial);
            depthFetchMaterial = CheckShaderAndCreateMaterial(depthFetchShader, depthFetchMaterial);
            creaseApplyMaterial = CheckShaderAndCreateMaterial(creaseApplyShader, creaseApplyMaterial);

            if (!isSupported) {
                ReportAutoDisable();
            }

            return isSupported;
        }
    }
}