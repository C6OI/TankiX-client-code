using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Camera))]
    public class OutlineRender : MonoBehaviour {
        [SerializeField] Shader outlineFinal;

        [Range(0f, 20f)] public float Intensity = 2f;

        [SerializeField] Camera helperCamera;

        Material _compositeMat;

        void Awake() {
            helperCamera.gameObject.SetActive(false);
            _compositeMat = new Material(outlineFinal);
        }

        public void OnEnable() {
            int qualityLevel = QualitySettings.GetQualityLevel();
            helperCamera.gameObject.SetActive(qualityLevel >= 2);
        }

        void OnDisable() {
            helperCamera.gameObject.SetActive(false);
        }

        void OnRenderImage(RenderTexture src, RenderTexture dst) {
            if (!helperCamera.gameObject.activeSelf) {
                _compositeMat.SetFloat("_Intensity", 0f);
                Graphics.Blit(src, dst, _compositeMat, 0);
            } else {
                _compositeMat.SetFloat("_Intensity", Intensity);
                Graphics.Blit(src, dst, _compositeMat, 0);
            }
        }

        void ClearScreen(RenderTexture dst) {
            RenderTexture source = new(1, 1, 0);
            Graphics.Blit(source, dst);
        }
    }
}