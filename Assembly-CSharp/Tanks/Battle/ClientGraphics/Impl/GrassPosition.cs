using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [Serializable]
    public struct GrassPosition {
        public Vector3 position;

        public Vector2 lightmapUV;

        public int lightmapId;
    }
}