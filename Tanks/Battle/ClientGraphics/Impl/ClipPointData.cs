using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public struct ClipPointData {
        public Vector2 point2D;

        public VertexData vertexData;

        public int index;

        public ClipPointData(VertexData vertexData) {
            point2D = vertexData.vertex;
            this.vertexData = vertexData;
            index = -1;
        }

        public ClipPointData ToDepthSpace() {
            point2D.x = vertexData.vertex.z;
            return this;
        }

        public static ClipPointData Lerp(ClipPointData from, ClipPointData to, float lerpFactor) {
            ClipPointData result = default;
            result.point2D = Vector2.LerpUnclamped(from.point2D, to.point2D, lerpFactor);
            result.vertexData = VertexData.Lerp(from.vertexData, to.vertexData, lerpFactor);
            return result;
        }
    }
}