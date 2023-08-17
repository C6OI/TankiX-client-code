using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class DecalProjection {
        public DecalProjection() {
            AtlasHTilesCount = AtlasVTilesCount = 1;
            SurfaceAtlasPositions = new int[5];
            Up = Vector3.up;
        }

        public Ray Ray { get; set; }

        public float HalfSize { get; set; }

        public Vector3 Up { get; set; }

        public float Distantion { get; set; }

        public int AtlasHTilesCount { get; set; }

        public int AtlasVTilesCount { get; set; }

        public int[] SurfaceAtlasPositions { get; set; }

        public RaycastHit ProjectionHit { get; set; }
    }
}