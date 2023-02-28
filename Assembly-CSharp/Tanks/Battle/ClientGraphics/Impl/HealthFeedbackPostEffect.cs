using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HealthFeedbackPostEffect : MonoBehaviour {
        const float MAX_DAMAGE_LEVEL = 100f;

        [SerializeField] Material mat;

        int damageID;

        float damageIntensity;

        public float DamageIntensity {
            get => damageIntensity;
            set {
                damageIntensity = value;
                mat.SetFloat(damageID, Mathf.Lerp(0f, 100f, damageIntensity));
            }
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest) {
            Graphics.Blit(src, dest, mat);
        }

        public void Init(Material sourceMaterial) {
            mat = Instantiate(sourceMaterial);
            damageID = Shader.PropertyToID("_damage_lvl");
            DamageIntensity = 0f;
        }
    }
}