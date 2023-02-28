using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Tool.TankViewer.API {
    [ExecuteInEditMode]
    public class Environment : MonoBehaviour {
        public Cubemap skyCube;

        public Cubemap specCube;

        public float camHDRExposure = 1f;

        public float ambientIntensity = 1f;

        public float reflectionIntensity = 1f;

        Material _skyboxMaterial;

        Material SkyboxMaterial {
            get {
                if (_skyboxMaterial == null) {
                    Shader shader = Shader.Find("Skybox/Cubemap");

                    if ((bool)shader) {
                        _skyboxMaterial = new Material(shader);
                        _skyboxMaterial.name = "Skybox";
                    } else {
                        Debug.LogError("Couldn't find " + shader.name + " shader");
                    }
                }

                return _skyboxMaterial;
            }
        }

        void Awake() { }

        public SphericalHarmonicsL2 ProjectCubeIntoSH(Cubemap src, int miplevel) {
            Vector3[] array = new Vector3[18] {
                new(0f, 0f, -1f),
                new(0f, -1f, 0f),
                new(-1f, 0f, 0f),
                new(0f, 0f, 1f),
                new(0f, -1f, 0f),
                new(1f, 0f, 0f),
                new(1f, 0f, 0f),
                new(0f, 0f, 1f),
                new(0f, -1f, 0f),
                new(1f, 0f, 0f),
                new(0f, 0f, -1f),
                new(0f, 1f, 0f),
                new(1f, 0f, 0f),
                new(0f, -1f, 0f),
                new(0f, 0f, -1f),
                new(-1f, 0f, 0f),
                new(0f, -1f, 0f),
                new(0f, 0f, 1f)
            };

            float num = 0f;
            SphericalHarmonicsL2 sphericalHarmonicsL = default;
            sphericalHarmonicsL.Clear();

            for (int i = 0; i < 6; i++) {
                Vector3 vector = array[i * 3];
                Vector3 vector2 = -array[i * 3 + 1];
                Vector3 vector3 = -array[i * 3 + 2];
                Color[] pixels = src.GetPixels((CubemapFace)i, miplevel);
                int num2 = src.width >> miplevel;

                if (num2 < 1) {
                    num2 = 1;
                }

                float num3 = -1f + 1f / num2;
                float num4 = 2f * (1f - 1f / num2) / (num2 - 1f);

                for (int j = 0; j < num2; j++) {
                    float num5 = j * num4 + num3;

                    for (int k = 0; k < num2; k++) {
                        Color color = pixels[k + j * num2];
                        float num6 = k * num4 + num3;
                        float num7 = 1f + num6 * num6 + num5 * num5;
                        float num8 = 4f / (Mathf.Sqrt(num7) * num7);
                        Vector3 vector4 = vector3 + vector * num6 + vector2 * num5;
                        vector4.Normalize();
                        Color color2 = color * color.a * 8f;
                        sphericalHarmonicsL.AddDirectionalLight(-vector4, QualitySettings.activeColorSpace != ColorSpace.Linear ? color2 : color2.linear, num8 * 0.5f);
                        num += num8;
                    }
                }
            }

            float num9 = 4f / num;
            return sphericalHarmonicsL * num9;
        }
    }
}