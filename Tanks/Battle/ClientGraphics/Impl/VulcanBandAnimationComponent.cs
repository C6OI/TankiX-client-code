using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanBandAnimationComponent : MonoBehaviour, Component {
        [SerializeField] int materialIndex = 1;

        [SerializeField] float speed = 1f;

        [SerializeField] float bandCooldownSec = 0.2f;

        [SerializeField] float partCount = 36f;

        [SerializeField] string[] textureNames = new string[6]
            { "_MainTex", "_PaintMap", "_FrostTex", "_HeatTex", "_SurfaceMap", "_BumpMap" };

        Material bandMaterial;

        float currentCooldown;

        float currentStepDistance;

        bool isEjectorEnabled;

        float offset;

        float stepLength;

        Entity vulcanEntity;

        void Awake() => enabled = false;

        void Update() {
            if (currentCooldown > 0f) {
                currentCooldown -= Time.deltaTime;
                return;
            }

            if (isEjectorEnabled) {
                isEjectorEnabled = false;
                ClientUnityIntegrationUtils.ExecuteInFlow(ProvideCaseEjectionEvent);
            }

            float num = speed * Time.deltaTime;
            currentStepDistance += num;
            offset += num;
            offset = Mathf.Repeat(offset, 1f);
            int num2 = textureNames.Length;

            for (int i = 0; i < num2; i++) {
                bandMaterial.SetTextureOffset(textureNames[i], new Vector2(offset, 0f));
            }

            if (Mathf.Abs(currentStepDistance) >= stepLength) {
                currentStepDistance = 0f;
                currentCooldown = bandCooldownSec;
                isEjectorEnabled = true;
            }
        }

        void OnEnable() {
            currentStepDistance = 0f;
            currentCooldown = 0f;
            isEjectorEnabled = true;
        }

        void ProvideCaseEjectionEvent(Engine engine) =>
            engine.NewEvent<CartridgeCaseEjectionEvent>().Attach(vulcanEntity).Schedule();

        public void InitBand(Renderer renderer, Entity entity) {
            vulcanEntity = entity;
            bandMaterial = renderer.materials[materialIndex];
            stepLength = 1f / partCount;
            offset = 0f;
        }

        public void StartBandAnimation() => enabled = true;

        public void StopBandAnimation() => enabled = false;
    }
}