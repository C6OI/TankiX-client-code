using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraInitSystem : ECSSystem {
        [OnEventFire]
        public void SetPostEffectsQuality(NodeAddedEvent e, SingleNode<CameraComponent> cameraNode) {
            Camera camera = cameraNode.component.Camera;
            camera.depthTextureMode = DepthTextureMode.Depth;
            ActivatePostEffects(camera);
        }

        void ActivatePostEffects(Camera camera) {
            string text = QualitySettings.names[QualitySettings.GetQualityLevel()];
            text = text.ToLower();
            PostEffectsSet[] components = camera.GetComponents<PostEffectsSet>();
            PostEffectsSet[] array = components;

            foreach (PostEffectsSet postEffectsSet in array) {
                if (postEffectsSet.qualityName != text) {
                    postEffectsSet.SetActive(false);
                }
            }

            PostEffectsSet[] array2 = components;

            foreach (PostEffectsSet postEffectsSet2 in array2) {
                if (postEffectsSet2.qualityName == text) {
                    postEffectsSet2.SetActive(true);
                    camera.depthTextureMode = postEffectsSet2.depthTextureMode;
                }
            }
        }
    }
}