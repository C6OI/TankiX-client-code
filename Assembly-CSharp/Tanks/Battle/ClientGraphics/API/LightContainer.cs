using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class LightContainer : MonoBehaviour {
        [SerializeField] Light[] lights;

        public float maxLightIntensity = 2f;

        public void SetIntensity(float intensity) {
            int num = lights.Length;

            for (int i = 0; i < num; i++) {
                Light light = lights[i];
                light.intensity = intensity;
            }
        }
    }
}