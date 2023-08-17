using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IsisRayEffectBehaviour : MonoBehaviour {
        const float TAU = (float)Math.PI * 2f;

        [Header("Assets")] [SerializeField] ParticleSystem expandingBlob;

        [SerializeField] ParticleSystem contractingBlob;

        [SerializeField] LineRenderer[] rays;

        [SerializeField] Material damagingBallMaterial;

        [SerializeField] Material damagingRayMaterial;

        [SerializeField] Material healingBallMaterial;

        [SerializeField] Material healingRayMaterial;

        [Header("Initialization parameters")] [SerializeField] [Space(1f)]
        int curvesCount = 5;

        [SerializeField] float minCurveMagnitude = 0.05f;

        [SerializeField] float maxCurveMagnitude = 0.1f;

        [Header("Dynamic parameters")] [Space(1f)] [SerializeField]
        float offsetToCamera = 0.5f;

        [SerializeField] float smoothingSpeed = 1f;

        [SerializeField] float[] textureTilings;

        [SerializeField] float[] textureOffsets;

        [SerializeField] float verticesSpacing = 0.5f;

        [SerializeField] float curveLength = 5f;

        [SerializeField] float curveSpeed = 0.5f;

        [SerializeField] Color healColor;

        [SerializeField] Color damageColor;

        [SerializeField] Material[] damagingRayMaterials;

        [SerializeField] Material[] healingRayMaterials;

        readonly Vector3[] bezierPoints = new Vector3[3];

        Animation contractingAnimation;

        ParticleSystemRenderer contractingBlobRenderer;

        Light contractingLight;

        AnimationCurve curve;

        Vector3 endLocalPosition;

        Animation expandingAnimation;

        ParticleSystemRenderer expandingBlobRenderer;

        Light expandingLight;

        Animation farAnimation;

        ParticleSystem farBlob;

        ParticleSystemRenderer farBlobRenderer;

        Light farLight;

        Animation nearAnimation;

        ParticleSystem nearBlob;

        ParticleSystemRenderer nearBlobRenderer;

        Light nearLight;

        float textureScrollDirection = 1f;

        void Update() {
            for (int i = 0; i < rays.Length; i++) {
                Vector2 mainTextureOffset = rays[i].sharedMaterial.mainTextureOffset;

                mainTextureOffset.x =
                    (mainTextureOffset.x + textureOffsets[i] * textureScrollDirection * Time.deltaTime) % 1f;

                rays[i].sharedMaterial.mainTextureOffset = mainTextureOffset;
            }
        }

        public void Init() {
            damagingRayMaterials = new Material[rays.Length];
            healingRayMaterials = new Material[rays.Length];

            for (int i = 0; i < rays.Length; i++) {
                damagingRayMaterials[i] = Instantiate(damagingRayMaterial);
                healingRayMaterials[i] = Instantiate(healingRayMaterial);
            }

            expandingBlobRenderer = expandingBlob.GetComponent<ParticleSystemRenderer>();
            contractingBlobRenderer = contractingBlob.GetComponent<ParticleSystemRenderer>();
            expandingLight = expandingBlob.GetComponent<Light>();
            contractingLight = contractingBlob.GetComponent<Light>();
            expandingAnimation = expandingBlob.GetComponent<Animation>();
            contractingAnimation = contractingBlob.GetComponent<Animation>();
            InitCurve();
            Hide();
        }

        public void Show() => EnableBlob(nearBlob, nearLight, nearAnimation);

        public void Hide() {
            for (int i = 0; i < rays.Length; i++) {
                rays[i].enabled = false;
            }

            SetHealingMode();
            DisableBlob(nearBlob, nearLight, nearAnimation);
            DisableBlob(farBlob, farLight, farAnimation);
            enabled = false;
        }

        public void EnableTargetForHealing() {
            enabled = true;
            EnableBlob(farBlob, farLight, farAnimation);
            SetHealingMode();

            for (int i = 0; i < rays.Length; i++) {
                rays[i].enabled = true;
            }
        }

        public void EnableTargetForDamaging() {
            enabled = true;
            EnableBlob(farBlob, farLight, farAnimation);
            SetDamagingMode();

            for (int i = 0; i < rays.Length; i++) {
                rays[i].enabled = true;
            }
        }

        public void DisableTarget() {
            for (int i = 0; i < rays.Length; i++) {
                rays[i].enabled = false;
            }

            SetHealingMode();
            DisableBlob(farBlob, farLight, farAnimation);
            enabled = false;
        }

        public void UpdateTargetPosition(Transform targetTransform, Vector3 targetLocalPosition, float[] speedMultipliers,
            float[] pointsRandomness) {
            bezierPoints[0] = transform.position;
            float speed = speedMultipliers[1] * smoothingSpeed;
            float speed2 = speedMultipliers[2] * smoothingSpeed;
            Vector3 from = targetTransform.InverseTransformPoint(transform.position);
            endLocalPosition = MovePoint(endLocalPosition, from, targetLocalPosition, speed2, pointsRandomness[2]);
            bezierPoints[2] = targetTransform.TransformPoint(endLocalPosition);
            Vector3 to = Vector3.Lerp(transform.position, bezierPoints[2], 0.5f);
            bezierPoints[1] = MovePoint(bezierPoints[1], transform.position, to, speed, pointsRandomness[1]);
            nearBlob.transform.position = bezierPoints[0];

            farBlob.transform.position =
                bezierPoints[2] + (Camera.main.transform.position - bezierPoints[2]).normalized * offsetToCamera;

            Vector3 lhs = bezierPoints[2] - bezierPoints[0];
            float magnitude = lhs.magnitude;
            Vector3 normalized = Vector3.Cross(lhs, Camera.main.transform.forward).normalized;
            float num = magnitude / (curveLength * curvesCount);
            int num2 = 1 + Mathf.CeilToInt(magnitude / verticesSpacing);

            for (int i = 0; i < rays.Length; i++) {
                rays[i].SetVertexCount(num2);

                for (int j = 0; j < num2; j++) {
                    float num3 = j / (float)(num2 - 1);
                    Vector3 vector = Bezier.PointOnCurve(num3, bezierPoints[0], bezierPoints[1], bezierPoints[2]);
                    float time = (num3 * num + textureScrollDirection * curveSpeed * Time.time) % 1f;
                    Vector3 vector2 = normalized * curve.Evaluate(time);
                    rays[i].SetPosition(j, vector + vector2);
                }

                Vector2 mainTextureScale = rays[i].sharedMaterial.mainTextureScale;
                mainTextureScale.x = magnitude / textureTilings[i];
                rays[i].sharedMaterial.mainTextureScale = mainTextureScale;
            }
        }

        Vector3 MovePoint(Vector3 moveThis, Vector3 from, Vector3 to, float speed, float randomness) {
            Vector3 vector = Quaternion.LookRotation(to - from) * Random.insideUnitCircle * randomness;
            return Vector3.MoveTowards(moveThis, to + vector, Time.deltaTime * speed);
        }

        void SetHealingMode() {
            textureScrollDirection = -1f;

            SetBlobs(expandingBlob,
                expandingBlobRenderer,
                expandingLight,
                expandingAnimation,
                contractingBlob,
                contractingBlobRenderer,
                contractingLight,
                contractingAnimation);

            nearBlobRenderer.sharedMaterial = healingBallMaterial;
            farBlobRenderer.sharedMaterial = healingBallMaterial;
            nearLight.color = healColor;
            farLight.color = healColor;

            for (int i = 0; i < rays.Length; i++) {
                rays[i].material = healingRayMaterials[i];
            }
        }

        void SetDamagingMode() {
            textureScrollDirection = 1f;

            SetBlobs(contractingBlob,
                contractingBlobRenderer,
                contractingLight,
                contractingAnimation,
                expandingBlob,
                expandingBlobRenderer,
                expandingLight,
                expandingAnimation);

            nearBlobRenderer.sharedMaterial = damagingBallMaterial;
            farBlobRenderer.sharedMaterial = damagingBallMaterial;
            nearLight.color = damageColor;
            farLight.color = damageColor;

            for (int i = 0; i < rays.Length; i++) {
                rays[i].material = damagingRayMaterials[i];
            }
        }

        void SetBlobs(ParticleSystem nearBlob, ParticleSystemRenderer nearBlobRenderer, Light nearLight,
            Animation nearAnimation, ParticleSystem farBlob, ParticleSystemRenderer farBlobRenderer, Light farLight,
            Animation farAnimation) {
            this.nearBlob = nearBlob;
            this.nearBlobRenderer = nearBlobRenderer;
            this.nearBlob.transform.localPosition = Vector3.zero;
            this.nearLight = nearLight;
            this.nearAnimation = nearAnimation;
            this.farBlob = farBlob;
            this.farBlobRenderer = farBlobRenderer;
            this.farBlob.transform.localPosition = Vector3.zero;
            this.farLight = farLight;
            this.farAnimation = farAnimation;
        }

        void InitCurve() {
            float value = Random.value;
            Keyframe[] array = new Keyframe[5 * curvesCount];

            for (int i = 0; i < array.Length; i++) {
                array[i].time = i / (float)(array.Length - 1);

                array[i].value = Mathf.Sin((array[i].time * curvesCount + value) * ((float)Math.PI * 2f)) *
                                 Random.Range(minCurveMagnitude, maxCurveMagnitude);
            }

            curve = new AnimationCurve(array);
        }

        static void EnableBlob(ParticleSystem blob, Light light, Animation animation) {
            blob.enableEmission = true;
            blob.Emit(1);
            blob.Play();
            light.enabled = true;
            animation.enabled = true;
        }

        static void DisableBlob(ParticleSystem blob, Light light, Animation animation) {
            blob.Stop();
            blob.Clear();
            blob.enableEmission = false;
            light.enabled = false;
            animation.enabled = false;
        }
    }
}