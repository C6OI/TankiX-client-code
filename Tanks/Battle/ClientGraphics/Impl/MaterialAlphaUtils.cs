using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public static class MaterialAlphaUtils {
        public const float MIN_ALPHA = 0f;

        public const float MAX_ALPHA = 1f;

        public static void SetAlpha(this Material material, float alpha) {
            float a = Mathf.Clamp(alpha, 0f, 1f);
            Color color = material.color;
            color.a = a;
            material.color = color;
        }

        public static void SetAlpha(this Material[] materials, float alpha) {
            foreach (Material material in materials) {
                material.SetAlpha(alpha);
            }
        }

        public static void SetOverrideTag(this Material[] materials, string tag, string value) {
            foreach (Material material in materials) {
                material.SetOverrideTag(tag, value);
            }
        }

        public static float GetAlpha(this Material material) => material.color.a;

        public static void SetFullTransparent(this Material material) => material.SetAlpha(0f);

        public static void SetFullOpacity(this Material material) => material.SetAlpha(1f);

        public static Material[] GetAllMaterials(GameObject gameObject) {
            Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
            int num = 0;

            for (int i = 0; i < componentsInChildren.Length; i++) {
                num += componentsInChildren[i].materials.Length;
            }

            int num2 = 0;
            Material[] array = new Material[num];

            for (int j = 0; j < componentsInChildren.Length; j++) {
                for (int k = 0; k < componentsInChildren[j].materials.Length; k++) {
                    array[num2++] = componentsInChildren[j].materials[k];
                }
            }

            return array;
        }

        public static Material GetMaterial(GameObject gameObject) =>
            gameObject.GetComponentsInChildren<Renderer>()[0].material;
    }
}