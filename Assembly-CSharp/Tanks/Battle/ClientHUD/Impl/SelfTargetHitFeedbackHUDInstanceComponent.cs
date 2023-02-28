using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Tanks.Battle.ClientHUD.Impl {
    public class SelfTargetHitFeedbackHUDInstanceComponent : BehaviourComponent {
        const float HEIGHT_ROOT = 5f;

        [SerializeField] EntityBehaviour entityBehaviour;

        [SerializeField] Vector2 minSize = new(150f, 200f);

        [SerializeField] Vector2 maxSize = new(300f, 400f);

        [SerializeField] Vector2 relativeSizeCoeff = new(0.1f, 0.2f);

        [SerializeField] float lengthPercent = 0.001f;

        [SerializeField] [Range(0f, 1f)] [HideInInspector]
        float lengthInterpolator;

        [SerializeField] RectTransform rootRectTransform;

        [SerializeField] RectTransform imageRectTransform;

        [SerializeField] Animator animator;

        [SerializeField] Image image;

        [SerializeField] int fps = 30;

        [SerializeField] int frameCount = 6;

        [SerializeField] [HideInInspector] float alpha;

        float animationFrameTime;

        float animationTimer;

        int appearID;

        int colorID;

        int currentFrameIndex;

        int disappearID;

        Entity entity;

        float frameOffset;

        float length;

        Material material;

        float requiredLength;

        float slopeAddition;

        int textureID;

        float width;

        public SelfTargetHitEffectHUDData InitialData { get; private set; }

        void Update() {
            float deltaTime = Time.deltaTime;

            if (animationTimer > 0f) {
                animationTimer -= deltaTime;
                return;
            }

            UpdateSpriteFrame();
            animationTimer = animationFrameTime;
        }

        void LateUpdate() {
            UpdateSize();
            Color value = new(1f, 1f, 1f, alpha);
            material.SetColor(colorID, value);
        }

        public void Init(Entity entity, SelfTargetHitEffectHUDData data) {
            InitTransform(data);
            InitMaterial();
            UpdateTransform(data);
            UpdateSpriteFrame();
            this.entity = entity;
            entityBehaviour.BuildEntity(entity);
            gameObject.SetActive(true);
        }

        void InitTransform(SelfTargetHitEffectHUDData data) {
            InitialData = data;
            Vector2 cnvSize = data.CnvSize;
            float num = Mathf.Min(cnvSize.x, cnvSize.y);
            width = Mathf.Clamp(num * relativeSizeCoeff.x, minSize.x, maxSize.x);
            lengthInterpolator = 1f;
            requiredLength = Mathf.Clamp(num * relativeSizeCoeff.y, minSize.y, Mathf.Min(maxSize.y, Mathf.Max(data.Length * lengthPercent, minSize.y)));
            length = 0f;
        }

        void InitMaterial() {
            animationTimer = 0f;
            animationFrameTime = 1f / fps;
            alpha = 0f;
            colorID = Shader.PropertyToID("_Color");
            material = Instantiate(image.material);
            image.material = material;
            textureID = Shader.PropertyToID("_MainTex");
            frameOffset = 1f / frameCount;
            material.SetTextureScale(textureID, new Vector2(frameOffset, 1f));
            currentFrameIndex = -1;
        }

        public void UpdateTransform(SelfTargetHitEffectHUDData data) {
            rootRectTransform.localPosition = data.BoundsPosCanvas;
            rootRectTransform.localRotation = Quaternion.LookRotation(Vector3.forward, data.UpwardsNrm);
            UpdateSlope(data);
            UpdateSize();
        }

        void UpdateSlope(SelfTargetHitEffectHUDData data) {
            float num = width * 0.5f;
            float axisAngle = GetAxisAngle(data.UpwardsNrm, Vector2.right);
            float axisAngle2 = GetAxisAngle(data.UpwardsNrm, Vector2.up);
            float num2 = !(Mathf.Abs(Mathf.Abs(data.BoundsPosition.x - 0.5f) - 0.5f) <= 0.001f) ? axisAngle : axisAngle2;
            float f = num / Mathf.Tan((float)Math.PI / 180f * num2);

            if (!float.IsInfinity(f) && !float.IsNaN(f)) {
                slopeAddition = f;
            }
        }

        void UpdateSpriteFrame() {
            int num = Random.Range(0, frameCount);

            if (num == currentFrameIndex) {
                num++;

                if (num >= frameCount) {
                    currentFrameIndex = 0;
                }
            } else {
                currentFrameIndex = num;
            }

            material.SetTextureOffset(textureID, new Vector2(currentFrameIndex * frameOffset, 0f));
        }

        void UpdateSize() {
            length = Mathf.Lerp(0f, requiredLength, lengthInterpolator);
            Vector2 vector = new(width, 5f);
            rootRectTransform.sizeDelta = vector;
            float num = length + slopeAddition;
            Vector2 sizeDelta = vector;
            sizeDelta.y = num;
            float y = num * 0.5f - slopeAddition;
            imageRectTransform.sizeDelta = sizeDelta;
            imageRectTransform.localPosition = new Vector3(0f, y, 0f);
        }

        float GetAxisAngle(Vector2 vec, Vector2 axis) {
            float num = Vector2.Angle(vec, axis);

            if (num > 90f) {
                num = 180f - num;
            }

            return num;
        }

        void OnDisappeared() {
            EngineService.Engine.DeleteEntity(entity);
        }
    }
}