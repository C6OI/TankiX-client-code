using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public static class TankMaterialPropertyNames {
        public static string COLORING = "_ColoringMap";

        public static int TEMPERATURE_ID = Shader.PropertyToID("_Temperature");

        public static int TRACKS_OFFSET = Shader.PropertyToID("_TracksOffset");

        public static int ALPHA = Shader.PropertyToID("_Alpha");
    }
}