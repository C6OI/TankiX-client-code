using System;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    public abstract class GenericDecalProjectorBaseComponent : MonoBehaviour {
        public LayerMask affectedLayers = -1;

        public bool affectInactiveRenderers;

        public bool affectOtherDecals;

        public bool skipUnreadableMeshes;

        public DetailsMode detailsMode;

        public AffectedDetail[] affectedDetails = new AffectedDetail[0];

        public float cullingAngle = 90f;

        public float meshOffset;

        public bool projectAfterOffset;

        public float normalsSmoothing;

        public int uv1RectangleIndex;

        public int uv2RectangleIndex;

        public Color vertexColor = Color.white;

        [SerializeField] float m_VertexColorBlending;

        Transform m_CachedTransform;

        public Transform CachedTransform {
            get {
                if (m_CachedTransform == null) {
                    m_CachedTransform = transform;
                }

                return m_CachedTransform;
            }
        }

        public float VertexColorBlending {
            get => m_VertexColorBlending;
            set {
                if (value < 0f || value > 1f) {
                    throw new ArgumentOutOfRangeException("The blend value has to be in [0.0f, 1.0f].");
                }

                m_VertexColorBlending = value;
            }
        }

        void OnEnable() => m_CachedTransform = GetComponent<Transform>();

        public GenericDecalsBase GetDecalsBase() {
            GenericDecalsBase genericDecalsBase = null;
            Transform parent = transform;

            while (parent != null && genericDecalsBase == null) {
                genericDecalsBase = parent.GetComponent<GenericDecalsBase>();
                parent = parent.parent;
            }

            return genericDecalsBase;
        }

        public Bounds WorldBounds() {
            Matrix4x4 matrix4x = UnscaledLocalToWorldMatrix();
            Vector3 vector = 0.5f * CachedTransform.localScale;
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

        Matrix4x4 UnscaledLocalToWorldMatrix() =>
            Matrix4x4.TRS(CachedTransform.position, CachedTransform.rotation, Vector3.one);
    }
}