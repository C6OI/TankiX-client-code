using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [AddComponentMenu("Image Effects/Displacement/Vortex")]
    [ExecuteInEditMode]
    public class Vortex : ImageEffectBase {
        public Vector2 radius = new(0.4f, 0.4f);

        public float angle = 50f;

        public Vector2 center = new(0.5f, 0.5f);

        void OnRenderImage(RenderTexture source, RenderTexture destination) =>
            ImageEffects.RenderDistortion(material, source, destination, angle, center, radius);
    }
}