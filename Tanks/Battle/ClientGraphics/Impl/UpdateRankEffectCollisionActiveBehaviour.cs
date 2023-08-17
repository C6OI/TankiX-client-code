using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectCollisionActiveBehaviour : MonoBehaviour {
        public bool IsReverse;

        public float TimeDelay;

        public bool IsLookAt;

        UpdateRankEffectSettings effectSettings;

        void Start() {
            GetEffectSettingsComponent(transform);

            if (IsReverse) {
                effectSettings.RegistreInactiveElement(gameObject, TimeDelay);
                gameObject.SetActive(false);
            } else {
                effectSettings.RegistreActiveElement(gameObject, TimeDelay);
            }

            if (IsLookAt) {
                effectSettings.CollisionEnter += effectSettings_CollisionEnter;
            }
        }

        void effectSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e) =>
            transform.LookAt(effectSettings.transform.position + e.Hit.normal);

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