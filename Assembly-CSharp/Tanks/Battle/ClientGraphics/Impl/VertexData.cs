using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public struct VertexData {
        public Vector3 vertex;

        public Vector3 normal;

        public SurfaceType surfaceType;

        public VertexData(Vector3 vertex, Vector3 normal, SurfaceType surfaceType) {
            this.vertex = vertex;
            this.normal = normal;
            this.surfaceType = surfaceType;
        }

        public static VertexData Lerp(VertexData from, VertexData to, float lerpFactor) {
            VertexData result = default;
            result.vertex = Vector3.LerpUnclamped(from.vertex, to.vertex, lerpFactor);
            result.normal = Vector3.LerpUnclamped(from.normal, to.normal, lerpFactor);
            result.surfaceType = from.surfaceType;
            return result;
        }
    }
}