using UnityEngine;

namespace Edelweiss.DecalSystem {
    public class WrappedSkinnedDecalProjector : SkinnedDecalProjectorBase {
        readonly Transform m_Transform;

        public WrappedSkinnedDecalProjector(SkinnedDecalProjectorComponent a_DecalProjector) {
            WrappedSkinnedDecalProjectorComponent = a_DecalProjector;
            m_Transform = WrappedSkinnedDecalProjectorComponent.transform;
        }

        public SkinnedDecalProjectorComponent WrappedSkinnedDecalProjectorComponent { get; }

        public override Vector3 Position => m_Transform.position;

        public override Quaternion Rotation => m_Transform.rotation;

        public override Vector3 Scale => m_Transform.localScale;

        public override float CullingAngle => WrappedSkinnedDecalProjectorComponent.cullingAngle;

        public override float MeshOffset => WrappedSkinnedDecalProjectorComponent.meshOffset;

        public override int UV1RectangleIndex => WrappedSkinnedDecalProjectorComponent.uv1RectangleIndex;

        public override int UV2RectangleIndex => WrappedSkinnedDecalProjectorComponent.uv2RectangleIndex;

        public override Color VertexColor => WrappedSkinnedDecalProjectorComponent.vertexColor;

        public override float VertexColorBlending => WrappedSkinnedDecalProjectorComponent.VertexColorBlending;
    }
}