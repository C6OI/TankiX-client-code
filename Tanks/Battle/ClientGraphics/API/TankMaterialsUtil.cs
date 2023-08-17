using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public static class TankMaterialsUtil {
        static readonly string MAIN_MATERIAL_SUFFIX = "main";

        static readonly string NITRO_DETAILS_MATERIAL_SUFFIX = "nitro";

        static readonly string DD_DETAILS_MATERIAL_SUFFIX = "dd";

        static readonly string TRACKS_MATERIAL_MARK = "tracks";

        public static bool IsMainMaterial(Material material) => MaterialNameContainsString(material, MAIN_MATERIAL_SUFFIX);

        public static bool IsTracksMaterial(Material material) => MaterialNameContainsString(material, TRACKS_MATERIAL_MARK);

        public static Material GetMaterialForNitroDetails(Renderer renderer) {
            Material[] materials = renderer.materials;
            Material[] array = materials;

            foreach (Material material in array) {
                if (MaterialNameContainsString(material, NITRO_DETAILS_MATERIAL_SUFFIX)) {
                    return material;
                }
            }

            return null;
        }

        public static Material GetMaterialForDoubleDamageDetails(Renderer renderer) {
            Material[] materials = renderer.materials;
            Material[] array = materials;

            foreach (Material material in array) {
                if (MaterialNameContainsString(material, DD_DETAILS_MATERIAL_SUFFIX)) {
                    return material;
                }
            }

            return null;
        }

        public static void SetColoringTexture(Renderer renderer, Texture texture) {
            Material mainMaterial = GetMainMaterial(renderer);
            mainMaterial.EnableKeyword("_COLORING_ON");
            mainMaterial.SetTexture(TankMaterialPropertyNames.COLORING, texture);
        }

        public static void SetTemperature(Renderer renderer, float temperature) {
            Material[] sharedMaterials = renderer.sharedMaterials;
            Material[] array = sharedMaterials;

            foreach (Material material in array) {
                if ((IsMainMaterial(material) || IsTracksMaterial(material)) &&
                    material.HasProperty(TankMaterialPropertyNames.TEMPERATURE_ID)) {
                    material.SetFloat(TankMaterialPropertyNames.TEMPERATURE_ID, temperature);
                }
            }
        }

        public static void SetAlpha(Renderer renderer, float alpha) {
            float value = Mathf.Clamp01(alpha);
            Material[] materials = renderer.materials;
            Material[] array = materials;

            foreach (Material material in array) {
                material.SetFloat(TankMaterialPropertyNames.ALPHA, value);
            }
        }

        public static float GetAlpha(Renderer renderer) => renderer.material.GetFloat(TankMaterialPropertyNames.ALPHA);

        public static Material GetTrackMaterial(Renderer renderer) {
            Material[] materials = renderer.materials;

            foreach (Material material in materials) {
                if (IsTracksMaterial(material)) {
                    return material;
                }
            }

            return null;
        }

        public static Material GetMainMaterial(Renderer renderer) {
            Material[] materials = renderer.materials;

            foreach (Material material in materials) {
                if (IsMainMaterial(material)) {
                    return material;
                }
            }

            return null;
        }

        static bool MaterialNameContainsString(Material material, string suffix) {
            string text = material.name.ToLower();

            if (text.Contains(suffix)) {
                return true;
            }

            return false;
        }
    }
}