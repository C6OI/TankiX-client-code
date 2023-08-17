using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class GlobalMapParams : MonoBehaviour {
        public Material skybox;

        [Range(0f, 8f)] public float ambientIntensity = 1f;

        [Range(0f, 1f)] public float reflectionIntensity = 1f;

        [Range(0f, 5f)] public float indirectIntensity = 1f;

        public bool fog;

        public FogMode fogMode = FogMode.Linear;

        public Color fogColor = new(0.63f, 0.88f, 1f);

        public float fogDensity = 0.25f;

        public float fogStartDistance = 50f;

        public float fogEndDistance = 2000f;

        void Start() {
            if (!enabled || !gameObject.activeInHierarchy) {
                DynamicGI.UpdateEnvironment();
                return;
            }

            RenderSettings.skybox = skybox;
            RenderSettings.ambientIntensity = ambientIntensity;
            RenderSettings.reflectionIntensity = reflectionIntensity;
            RenderSettings.fog = fog;
            RenderSettings.fogMode = fogMode;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogStartDistance = fogStartDistance;
            RenderSettings.fogEndDistance = fogEndDistance;
            RenderSettings.fogDensity = fogDensity;
            DynamicGI.indirectScale = indirectIntensity;
            DynamicGI.UpdateEnvironment();
        }

        void OnValidate() => Start();

        [ContextMenu("Grab rendering settings")]
        public void GrabRenderingSettings() {
            skybox = RenderSettings.skybox;
            ambientIntensity = RenderSettings.ambientIntensity;
            reflectionIntensity = RenderSettings.reflectionIntensity;
            fog = RenderSettings.fog;
            fogMode = RenderSettings.fogMode;
            fogColor = RenderSettings.fogColor;
            fogStartDistance = RenderSettings.fogStartDistance;
            fogEndDistance = RenderSettings.fogEndDistance;
            fogDensity = RenderSettings.fogDensity;
            indirectIntensity = DynamicGI.indirectScale;
        }
    }
}