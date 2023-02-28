using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectRotateAround : MonoBehaviour {
        public float Speed = 1f;

        public float LifeTime = 1f;

        public float TimeDelay;

        public float SpeedFadeInTime;

        public bool UseCollision;

        public UpdateRankEffectSettings EffectSettings;

        float allTime;

        bool canUpdate;

        float currentSpeedFadeIn;

        void Start() {
            if (UseCollision) {
                EffectSettings.CollisionEnter += EffectSettings_CollisionEnter;
            }

            if (TimeDelay > 0f) {
                Invoke("ChangeUpdate", TimeDelay);
            } else {
                canUpdate = true;
            }
        }

        void Update() {
            if (!canUpdate) {
                return;
            }

            allTime += Time.deltaTime;

            if (allTime >= LifeTime && LifeTime > 0.0001f) {
                return;
            }

            if (SpeedFadeInTime > 0.001f) {
                if (currentSpeedFadeIn < Speed) {
                    currentSpeedFadeIn += Time.deltaTime / SpeedFadeInTime * Speed;
                } else {
                    currentSpeedFadeIn = Speed;
                }
            } else {
                currentSpeedFadeIn = Speed;
            }

            transform.Rotate(Vector3.forward * Time.deltaTime * currentSpeedFadeIn);
        }

        void OnEnable() {
            canUpdate = true;
            allTime = 0f;
        }

        void EffectSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e) {
            canUpdate = false;
        }

        void ChangeUpdate() {
            canUpdate = true;
        }
    }
}