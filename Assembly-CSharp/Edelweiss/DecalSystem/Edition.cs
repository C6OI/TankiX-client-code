using UnityEngine;

namespace Edelweiss.DecalSystem {
    public class Edition {
        public static bool IsDecalSystemFree => false;

        public static bool IsDX11 => SystemInfo.graphicsShaderLevel >= 41;
    }
}