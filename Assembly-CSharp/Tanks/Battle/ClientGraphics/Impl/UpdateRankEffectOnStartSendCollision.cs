using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectOnStartSendCollision : MonoBehaviour {
        UpdateRankEffectSettings effectSettings;

        bool isInitialized;

        void Start() {
            GetEffectSettingsComponent(transform);
            effectSettings.OnCollisionHandler(new UpdateRankCollisionInfo());
            isInitialized = true;
        }

        void OnEnable() {
            if (isInitialized) {
                effectSettings.OnCollisionHandler(new UpdateRankCollisionInfo());
            }
        }

        void GetEffectSettingsComponent(Transform tr) {
            Transform parent = tr.parent;

            if (parent != null) {
                effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();

                if (effectSettings == null) {
                    GetEffectSettingsComponent(parent.transform);
                }
            }
        }
    }
}