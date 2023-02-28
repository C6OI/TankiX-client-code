using UnityEngine;

namespace Edelweiss.DecalSystem {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class DecalsMeshRenderer : MonoBehaviour {
        [HideInInspector] [SerializeField] MeshFilter m_MeshFilter;

        [HideInInspector] [SerializeField] MeshRenderer m_MeshRenderer;

        public MeshFilter MeshFilter {
            get {
                if (m_MeshFilter == null) {
                    m_MeshFilter = GetComponent<MeshFilter>();
                }

                return m_MeshFilter;
            }
        }

        public MeshRenderer MeshRenderer {
            get {
                if (m_MeshRenderer == null) {
                    m_MeshRenderer = GetComponent<MeshRenderer>();
                }

                return m_MeshRenderer;
            }
        }
    }
}