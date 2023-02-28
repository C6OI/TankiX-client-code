using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ColoringComponent : BehaviourComponent {
        public enum COLORING_MAP_ALPHA_MODE {
            NONE = -1,
            AS_EMISSION_MASK = 1,
            AS_SMOOTHNESS = 2
        }

        public Color32 color = Color.white;

        public Texture2D coloringTexture;

        public COLORING_MAP_ALPHA_MODE coloringTextureAlphaMode = COLORING_MAP_ALPHA_MODE.NONE;

        public Texture2D coloringNormalMap;

        public float coloringNormalScale = 1f;

        public float metallic = 0.3f;

        public bool overwriteSmoothness;

        public float smoothnessStrength;

        public bool useColoringIntensityThreshold;

        public float coloringMaskThreshold;

        public bool overrideEmission;

        public Color emissionColor = Color.HSVToRGB(0.347f, 0.917f, 0.471f);

        public float emissionIntensity = 1f;
    }
}