using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class MaskImageEffect : MonoBehaviour {
        public Shader shader;

        Material material;

        void Awake() => material = new Material(shader);

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination) => Graphics.Blit(source, destination, material);
    }
}