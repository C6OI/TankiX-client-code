using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
    [ExecuteInEditMode]
    public class Grayscale : ImageEffectBase {
        public Texture textureRamp;

        public float rampOffset;

        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            material.SetTexture("_RampTex", textureRamp);
            material.SetFloat("_RampOffset", rampOffset);
            Graphics.Blit(source, destination, material);
        }
    }
}