using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectSetPositionOnHit : MonoBehaviour {
        public float OffsetPosition;

        UpdateRankEffectSettings effectSettings;

        bool isInitialized;

        Transform tRoot;

        void Start() {
            GetEffectSettingsComponent(transform);

            if (effectSettings == null) {
                Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
            }

            tRoot = effectSettings.transform;
        }

        void Update() {
            if (!isInitialized) {
                isInitialized = true;
                effectSettings.CollisionEnter += effectSettings_CollisionEnter;
            }
        }

        void OnDisable() {
            transform.position = Vector3.zero;
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

        void effectSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e) {
            Vector3 normalized = (tRoot.position + Vector3.Normalize(e.Hit.point - tRoot.position) * (effectSettings.MoveDistance + 1f)).normalized;
            transform.position = e.Hit.point - normalized * OffsetPosition;
        }
    }
}