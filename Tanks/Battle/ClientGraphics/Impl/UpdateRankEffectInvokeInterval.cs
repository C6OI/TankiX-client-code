using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectInvokeInterval : MonoBehaviour {
        public GameObject GO;

        public float Interval = 0.3f;

        public float Duration = 3f;

        int count;

        UpdateRankEffectSettings effectSettings;

        int goIndexActivate;

        int goIndexDeactivate;

        List<GameObject> goInstances;

        bool isInitialized;

        void Start() {
            GetEffectSettingsComponent(transform);
            goInstances = new List<GameObject>();
            count = (int)(Duration / Interval);

            for (int i = 0; i < count; i++) {
                GameObject gameObject = Instantiate(GO, transform.position, default);
                gameObject.transform.parent = transform;
                UpdateRankEffectSettings component = gameObject.GetComponent<UpdateRankEffectSettings>();
                component.Target = effectSettings.Target;
                component.IsHomingMove = effectSettings.IsHomingMove;
                component.MoveDistance = effectSettings.MoveDistance;
                component.MoveSpeed = effectSettings.MoveSpeed;

                component.CollisionEnter += delegate(object n, UpdateRankCollisionInfo e) {
                    effectSettings.OnCollisionHandler(e);
                };

                component.ColliderRadius = effectSettings.ColliderRadius;
                component.EffectRadius = effectSettings.EffectRadius;
                component.EffectDeactivated += effectSettings_EffectDeactivated;
                goInstances.Add(gameObject);
                gameObject.SetActive(false);
            }

            InvokeAll();
            isInitialized = true;
        }

        void OnEnable() {
            if (isInitialized) {
                InvokeAll();
            }
        }

        void OnDisable() { }

        void GetEffectSettingsComponent(Transform tr) {
            Transform parent = tr.parent;

            if (parent != null) {
                effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();

                if (effectSettings == null) {
                    GetEffectSettingsComponent(parent.transform);
                }
            }
        }

        void InvokeAll() {
            for (int i = 0; i < count; i++) {
                Invoke("InvokeInstance", i * Interval);
            }
        }

        void InvokeInstance() {
            goInstances[goIndexActivate].SetActive(true);

            if (goIndexActivate >= goInstances.Count - 1) {
                goIndexActivate = 0;
            } else {
                goIndexActivate++;
            }
        }

        void effectSettings_EffectDeactivated(object sender, EventArgs e) {
            UpdateRankEffectSettings updateRankEffectSettings = sender as UpdateRankEffectSettings;
            updateRankEffectSettings.transform.position = transform.position;

            if (goIndexDeactivate >= count - 1) {
                effectSettings.Deactivate();
                goIndexDeactivate = 0;
            } else {
                goIndexDeactivate++;
            }
        }
    }
}