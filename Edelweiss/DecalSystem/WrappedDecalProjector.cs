using UnityEngine;

namespace Edelweiss.DecalSystem {
    public class WrappedDecalProjector : DecalProjectorBase {
        readonly Transform m_Transform;

        public WrappedDecalProjector(DecalProjectorComponent a_DecalProjector) {
            WrappedDecalProjectorComponent = a_DecalProjector;
            m_Transform = WrappedDecalProjectorComponent.transform;
        }

        public DecalProjectorComponent WrappedDecalProjectorComponent { get; }

        public override Vector3 Position => m_Transform.position;

        public override Quaternion Rotation => m_Transform.rotation;

        public override Vector3 Scale => m_Transform.localScale;

        public override float CullingAngle => WrappedDecalProjectorComponent.cullingAngle;

        public override float MeshOffset => WrappedDecalProjectorComponent.meshOffset;

        public override int UV1RectangleIndex => WrappedDecalProjectorComponent.uv1RectangleIndex;

        public override int UV2RectangleIndex => WrappedDecalProjectorComponent.uv2RectangleIndex;

        public override Color VertexColor => WrappedDecalProjectorComponent.vertexColor;

        public override float VertexColorBlending => WrappedDecalProjectorComponent.VertexColorBlending;
    }
}