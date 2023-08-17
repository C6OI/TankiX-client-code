using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Displacement/Fisheye")]
    public class Fisheye : PostEffectsBase {
        public float strengthX = 0.05f;

        public float strengthY = 0.05f;

        public Shader fishEyeShader;

        Material fisheyeMaterial;

        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (!CheckResources()) {
                Graphics.Blit(source, destination);
                return;
            }

            float num = 5f / 32f;
            float num2 = source.width * 1f / (source.height * 1f);

            fisheyeMaterial.SetVector("intensity",
                new Vector4(strengthX * num2 * num, strengthY * num, strengthX * num2 * num, strengthY * num));

            Graphics.Blit(source, destination, fisheyeMaterial);
        }

        public override bool CheckResources() {
            CheckSupport(false);
            fisheyeMaterial = CheckShaderAndCreateMaterial(fishEyeShader, fisheyeMaterial);

            if (!isSupported) {
                ReportAutoDisable();
            }

            return isSupported;
        }
    }
}