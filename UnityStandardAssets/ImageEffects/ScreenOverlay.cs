using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [AddComponentMenu("Image Effects/Other/Screen Overlay")]
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class ScreenOverlay : PostEffectsBase {
        public enum OverlayBlendMode {
            Additive = 0,
            ScreenBlend = 1,
            Multiply = 2,
            Overlay = 3,
            AlphaBlend = 4
        }

        public OverlayBlendMode blendMode = OverlayBlendMode.Overlay;

        public float intensity = 1f;

        public Texture2D texture;

        public Shader overlayShader;

        Material overlayMaterial;

        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (!CheckResources()) {
                Graphics.Blit(source, destination);
                return;
            }

            Vector4 vector = new(1f, 0f, 0f, 1f);
            overlayMaterial.SetVector("_UV_Transform", vector);
            overlayMaterial.SetFloat("_Intensity", intensity);
            overlayMaterial.SetTexture("_Overlay", texture);
            Graphics.Blit(source, destination, overlayMaterial, (int)blendMode);
        }

        public override bool CheckResources() {
            CheckSupport(false);
            overlayMaterial = CheckShaderAndCreateMaterial(overlayShader, overlayMaterial);

            if (!isSupported) {
                ReportAutoDisable();
            }

            return isSupported;
        }
    }
}