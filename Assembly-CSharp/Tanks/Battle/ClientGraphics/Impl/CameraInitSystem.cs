using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraInitSystem : ECSSystem {
        [OnEventFire]
        public void SetPostEffectsQuality(NodeAddedEvent e, SingleNode<CameraComponent> cameraNode) {
            CameraComponent component = cameraNode.component;
            component.DepthTextureMode = DepthTextureMode.Depth;
            ActivatePostEffects(component);
        }

        void ActivatePostEffects(CameraComponent camera) {
            string text = QualitySettings.names[QualitySettings.GetQualityLevel()];
            text = text.ToLower();
            PostEffectsSet[] postEffectsSets = camera.PostEffectsSets;
            PostEffectsSet[] array = postEffectsSets;

            foreach (PostEffectsSet postEffectsSet in array) {
                if (postEffectsSet.qualityName != text) {
                    postEffectsSet.SetActive(false);
                }
            }

            PostEffectsSet[] array2 = postEffectsSets;

            foreach (PostEffectsSet postEffectsSet2 in array2) {
                if (postEffectsSet2.qualityName == text) {
                    postEffectsSet2.SetActive(true);
                    camera.DepthTextureMode = postEffectsSet2.depthTextureMode;
                }
            }
        }
    }
}