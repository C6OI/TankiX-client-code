using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public static class ClientGraphicsUtil {
        public static void UpdateVector(Renderer renderer, string propertyName, Vector4 vector) {
            for (int i = 0; i < renderer.materials.Length; i++) {
                Material material = renderer.materials[i];
                material.SetVector(propertyName, vector);
            }
        }

        public static void UpdateAlpha(Material material, string propertyName, float alpha) {
            Color color = material.GetColor(propertyName);
            Color value = new(color.r, color.g, color.b, alpha);
            material.SetColor(propertyName, value);
        }

        public static void ApplyShaderToRenderer(Renderer renderer, Shader shader) {
            for (int i = 0; i < renderer.materials.Length; i++) {
                Material material = renderer.materials[i];
                material.shader = shader;
            }
        }
    }
}