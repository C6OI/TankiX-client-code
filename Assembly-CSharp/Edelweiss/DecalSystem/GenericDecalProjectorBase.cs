using UnityEngine;

namespace Edelweiss.DecalSystem {
    public abstract class GenericDecalProjectorBase {
        public bool IsActiveProjector { get; internal set; }

        public int DecalsMeshLowerVertexIndex { get; internal set; }

        public int DecalsMeshUpperVertexIndex { get; internal set; }

        public int DecalsMeshVertexCount => DecalsMeshUpperVertexIndex - DecalsMeshLowerVertexIndex + 1;

        public int DecalsMeshLowerTriangleIndex { get; internal set; }

        public int DecalsMeshUpperTriangleIndex { get; internal set; }

        public int DecalsMeshTriangleCount => DecalsMeshUpperTriangleIndex - DecalsMeshLowerTriangleIndex + 1;

        public bool IsUV1ProjectionCalculated { get; internal set; }

        public bool IsUV2ProjectionCalculated { get; internal set; }

        public bool IsTangentProjectionCalculated { get; internal set; }

        public abstract Vector3 Position { get; }

        public abstract Quaternion Rotation { get; }

        public abstract Vector3 Scale { get; }

        public abstract float CullingAngle { get; }

        public abstract float MeshOffset { get; }

        public abstract int UV1RectangleIndex { get; }

        public abstract int UV2RectangleIndex { get; }

        public abstract Color VertexColor { get; }

        public abstract float VertexColorBlending { get; }

        public Matrix4x4 ProjectorToWorldMatrix => Matrix4x4.TRS(Position, Rotation, Scale);

        public Matrix4x4 WorldToProjectorMatrix => ProjectorToWorldMatrix.inverse;

        public Bounds WorldBounds() {
            Matrix4x4 matrix4x = Matrix4x4.TRS(Position, Rotation, Vector3.one);
            Vector3 vector = 0.5f * Scale;
            Vector3 vector2 = new(0f, 0f - Mathf.Abs(vector.y), 0f);
            Vector3 center = matrix4x.MultiplyPoint3x4(Vector3.zero);
            Bounds result = new(center, Vector3.zero);
            center = vector2 + new Vector3(vector.x, vector.y, vector.z);
            center = matrix4x.MultiplyPoint3x4(center);
            result.Encapsulate(center);
            center = vector2 + new Vector3(vector.x, vector.y, 0f - vector.z);
            center = matrix4x.MultiplyPoint3x4(center);
            result.Encapsulate(center);
            center = vector2 + new Vector3(vector.x, 0f - vector.y, vector.z);
            center = matrix4x.MultiplyPoint3x4(center);
            result.Encapsulate(center);
            center = vector2 + new Vector3(vector.x, 0f - vector.y, 0f - vector.z);
            center = matrix4x.MultiplyPoint3x4(center);
            result.Encapsulate(center);
            center = vector2 + new Vector3(0f - vector.x, vector.y, vector.z);
            center = matrix4x.MultiplyPoint3x4(center);
            result.Encapsulate(center);
            center = vector2 + new Vector3(0f - vector.x, vector.y, 0f - vector.z);
            center = matrix4x.MultiplyPoint3x4(center);
            result.Encapsulate(center);
            center = vector2 + new Vector3(0f - vector.x, 0f - vector.y, vector.z);
            center = matrix4x.MultiplyPoint3x4(center);
            result.Encapsulate(center);
            center = vector2 + new Vector3(0f - vector.x, 0f - vector.y, 0f - vector.z);
            center = matrix4x.MultiplyPoint3x4(center);
            result.Encapsulate(center);
            return result;
        }
    }
}