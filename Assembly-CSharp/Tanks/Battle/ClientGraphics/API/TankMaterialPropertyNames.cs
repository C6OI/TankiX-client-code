using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public static class TankMaterialPropertyNames {
        public static int COLORING_MAP = Shader.PropertyToID("_ColoringMap");

        public static int COLORING_BUMP = Shader.PropertyToID("_ColoringBumpMap");

        public static int COLORING_ID = Shader.PropertyToID("_Coloring");

        public static int METALLIC_COLORING_ID = Shader.PropertyToID("_MetallicColoring");

        public static int COLORING_BUMP_SCALE_ID = Shader.PropertyToID("_ColoringBumpScale");

        public static int COLORING_SMOOTHNESS_ID = Shader.PropertyToID("_ColoringSmoothness");

        public static int COLORING_MASK_THRESHOLD_ID = Shader.PropertyToID("_ColoringMaskThreshold");

        public static int COLORING_BUMP_MAP_DEF_ID = Shader.PropertyToID("_ColoringBumpMapDef");

        public static int COLORING_MAP_ALPHA_DEF_ID = Shader.PropertyToID("_ColoringMapAlphaDef");

        public static int TEMPERATURE_ID = Shader.PropertyToID("_Temperature");

        public static int TRACKS_OFFSET = Shader.PropertyToID("_TracksOffset");

        public static int ALPHA = Shader.PropertyToID("_Alpha");

        public static int EMISSION_COLOR_ID = Shader.PropertyToID("_EmissionColor");

        public static int EMISSION_INTENSITY_ID = Shader.PropertyToID("_EmissionIntensity");

        public static string COLORING_KEYWORD = "_COLORING_ON";
    }
}