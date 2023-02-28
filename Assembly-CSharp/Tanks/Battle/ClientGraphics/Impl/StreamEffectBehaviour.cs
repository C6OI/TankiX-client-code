using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class StreamEffectBehaviour : MonoBehaviour {
        const float STRAIGHT_DIR = 1f;

        const float REVERSE_DIR = -1f;

        [SerializeField] float range;

        [SerializeField] AnimationCurve nearIntensityEasing;

        [SerializeField] AnimationCurve farIntensityEasing;

        [SerializeField] Light nearLight;

        [SerializeField] Light farLight;

        [SerializeField] float farLightDefaultSpeed = 1f;

        [SerializeField] float farLightMaxSpeed = 4f;

        float easingDirection;

        Animator farAnimator;

        float farIntensityTime;

        Vector3 farLightStartPosition;

        bool lightIsEnabled = true;

        Animator nearAnimator;

        float nearIntensityTime;

        ParticleSystem particleSystem;

        public float Range {
            get => range;
            set => range = value;
        }

        public void Awake() {
            particleSystem = GetComponent<ParticleSystem>();
            nearAnimator = nearLight.GetComponent<Animator>();
            farAnimator = farLight.GetComponent<Animator>();
            farLightStartPosition = farLight.transform.localPosition;
            Stop();
        }

        void Update() {
            float num = range + farLightStartPosition.z;
            Vector3 end = transform.position + transform.forward * num;
            float num2 = farLightDefaultSpeed;
            RaycastHit hitInfo;

            if (Physics.Linecast(transform.position - transform.forward, end, out hitInfo, LayerMasks.VISUAL_STATIC)) {
                num2 = farLightMaxSpeed;
                num = Vector3.Distance(transform.position, hitInfo.point);
            }

            Vector3 localPosition = farLight.transform.localPosition;
            localPosition.z = Mathf.Lerp(localPosition.z, num, Time.deltaTime * num2);
            farLight.transform.localPosition = localPosition;
        }

        void LateUpdate() {
            float deltaTime = Time.deltaTime;
            float num = float.Epsilon;
            nearIntensityTime = Mathf.Clamp(nearIntensityTime + easingDirection * deltaTime, 0f, nearIntensityEasing.keys[nearIntensityEasing.keys.Length - 1].time);
            farIntensityTime = Mathf.Clamp(farIntensityTime + easingDirection * deltaTime, 0f, farIntensityEasing.keys[farIntensityEasing.keys.Length - 1].time);
            float num2 = nearIntensityEasing.Evaluate(nearIntensityTime);
            float num3 = farIntensityEasing.Evaluate(farIntensityTime);
            nearLight.enabled = num2 > num;
            farLight.enabled = num3 > num;
            nearLight.intensity *= num2;
            farLight.intensity *= num3;
            nearAnimator.enabled = nearLight.enabled;
            farAnimator.enabled = farLight.enabled;
            enabled = nearLight.enabled || farLight.enabled;
        }

        public void Play() {
            farLight.transform.localPosition = farLightStartPosition;
            particleSystem.Play(true);
            easingDirection = 1f;
            enabled = lightIsEnabled;
        }

        public void Stop() {
            enabled = lightIsEnabled;
            easingDirection = -1f;

            if ((bool)particleSystem) {
                particleSystem.Stop(true);
            }
        }

        public virtual void ApplySettings(StreamWeaponSettingsComponent streamWeaponSettings) {
            if (!streamWeaponSettings.LightIsEnabled) {
                lightIsEnabled = false;
                farLight.enabled = false;
                nearLight.enabled = false;
                enabled = false;
            }
        }

        public virtual void AddCollisionLayer(int layer) { }
    }
}