using System;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    public class SkinnedDecalProjector : SkinnedDecalProjectorBase {
        public float cullingAngle;

        float m_VertexColorBlending;

        public float meshOffset;
        public Vector3 position;

        public Quaternion rotation;

        public Vector3 scale;

        public int uv1RectangleIndex;

        public int uv2RectangleIndex;

        public Color vertexColor;

        public SkinnedDecalProjector(Vector3 a_Position, Quaternion a_Rotation, Vector3 a_Scale, float a_CullingAngle,
            float a_meshOffset, int a_UV1RectangleIndex, int a_UV2RectangleIndex) {
            position = a_Position;
            rotation = a_Rotation;
            scale = a_Scale;
            cullingAngle = a_CullingAngle;
            meshOffset = a_meshOffset;
            uv1RectangleIndex = a_UV1RectangleIndex;
            uv2RectangleIndex = a_UV2RectangleIndex;
            vertexColor = Color.white;
            SetVertexColorBlending(0f);
        }

        public SkinnedDecalProjector(Vector3 a_Position, Quaternion a_Rotation, Vector3 a_Scale, float a_CullingAngle,
            float a_meshOffset, int a_UV1RectangleIndex, int a_UV2RectangleIndex, Color a_VertexColor,
            float a_VertexColorBlending) {
            position = a_Position;
            rotation = a_Rotation;
            scale = a_Scale;
            cullingAngle = a_CullingAngle;
            meshOffset = a_meshOffset;
            uv1RectangleIndex = a_UV1RectangleIndex;
            uv2RectangleIndex = a_UV2RectangleIndex;
            vertexColor = a_VertexColor;
            SetVertexColorBlending(a_VertexColorBlending);
        }

        public override Vector3 Position => position;

        public override Quaternion Rotation => rotation;

        public override Vector3 Scale => scale;

        public override float CullingAngle => cullingAngle;

        public override float MeshOffset => meshOffset;

        public override int UV1RectangleIndex => uv1RectangleIndex;

        public override int UV2RectangleIndex => uv2RectangleIndex;

        public override Color VertexColor => vertexColor;

        public override float VertexColorBlending => m_VertexColorBlending;

        public void SetVertexColorBlending(float a_VertexColorBlending) {
            if (a_VertexColorBlending < 0f || a_VertexColorBlending > 1f) {
                throw new ArgumentOutOfRangeException("The blend value has to be in [0.0f, 1.0f].");
            }

            m_VertexColorBlending = a_VertexColorBlending;
        }
    }
}