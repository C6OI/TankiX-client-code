using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [AddComponentMenu("")]
    [RequireComponent(typeof(Camera))]
    public class ImageEffectBase : MonoBehaviour {
        public Shader shader;

        Material m_Material;

        protected Material material {
            get {
                if (m_Material == null) {
                    m_Material = new Material(shader);
                    m_Material.hideFlags = HideFlags.HideAndDontSave;
                }

                return m_Material;
            }
        }

        protected virtual void Start() {
            if (!SystemInfo.supportsImageEffects) {
                enabled = false;
            } else if (!shader || !shader.isSupported) {
                enabled = false;
            }
        }

        protected virtual void OnDisable() {
            if ((bool)m_Material) {
                DestroyImmediate(m_Material);
            }
        }
    }
}