using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public static class ClientGraphicsUtil {
        public static void UpdateVector(Renderer renderer, string propertyName, Vector4 vector) {
            Material[] materials = renderer.materials;

            foreach (Material material in materials) {
                material.SetVector(propertyName, vector);
            }
        }

        public static void UpdateAlpha(Material material, string propertyName, float alpha) {
            Color color = material.GetColor(propertyName);
            Color color2 = new(color.r, color.g, color.b, alpha);
            material.SetColor(propertyName, color2);
        }

        public static void ApplyShaderToRenderer(Renderer renderer, Shader shader) {
            Material[] materials = renderer.materials;

            foreach (Material material in materials) {
                material.shader = shader;
            }
        }
    }
}