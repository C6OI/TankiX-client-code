using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using UnityEngine.Rendering;
using GraphicsSettings = Tanks.Lobby.ClientSettings.API.GraphicsSettings;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapInitSystem : ECSSystem {
        [OnEventFire]
        public void SetMaterialsQuality(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<MaterialsSettingsComponent> materialsSettings) {
            Shader.globalMaximumLOD = materialsSettings.component.GlobalShadersMaximumLOD;
        }

        [OnEventFire]
        public void SetGrassQuality(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, SingleNode<CameraComponent> cameraNode) {
            GraphicsSettings iNSTANCE = GraphicsSettings.INSTANCE;

            if (iNSTANCE.CurrentGrassFarDrawDistance > 0.1f) {
                ShadowCastingMode mode = iNSTANCE.CurrentGrassCastsShadow ? ShadowCastingMode.On : ShadowCastingMode.Off;
                GrassGenerator[] componentsInChildren = map.component.SceneRoot.GetComponentsInChildren<GrassGenerator>();
                GrassGenerator[] array = componentsInChildren;

                foreach (GrassGenerator grassGenerator in array) {
                    grassGenerator.SetCulling(iNSTANCE.CurrentGrassFarDrawDistance, iNSTANCE.CurrentGrassNearDrawDistance, iNSTANCE.CurrentGrassFadeRange,
                        iNSTANCE.CurrentGrassDensityMultiplier);

                    grassGenerator.Generate();
                    SetShadowCastingMode(grassGenerator.transform, mode);
                }
            }

            map.component.SceneRoot.AddComponent<ShadowCasterCreatorBehaviour>();
        }

        [OnEventFire]
        public void SetPostProcessing(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, SingleNode<CameraComponent> cameraNode,
            SingleNode<PostProcessingQualityVariantComponent> settings) {
            GraphicsSettings.INSTANCE.customSettings = settings.component.CustomSettings;
            GraphicsSettings.INSTANCE.currentAmbientOcclusion = settings.component.AmbientOcclusion;
            GraphicsSettings.INSTANCE.currentBloom = settings.component.Bloom;
            GraphicsSettings.INSTANCE.currentChromaticAberration = settings.component.ChromaticAberration;
            GraphicsSettings.INSTANCE.currentGrain = settings.component.Grain;
            GraphicsSettings.INSTANCE.currentVignette = settings.component.Vignette;
        }

        [OnEventFire]
        public void SetWaterQuality(NodeAddedEvent e, SingleNode<WaterComponent> water, [JoinAll] SingleNode<WaterSettingsComponent> waterSettings) {
            if (!waterSettings.component.HasReflection) {
                water.component.DisableReflection();
            }

            water.component.EdgeBlend = waterSettings.component.EdgeBlend;
        }

        void SetShadowCastingMode(Transform root, ShadowCastingMode mode) {
            MeshRenderer[] componentsInChildren = root.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer[] array = componentsInChildren;

            foreach (MeshRenderer meshRenderer in array) {
                meshRenderer.shadowCastingMode = mode;
            }
        }

        public class CameraNode : Node {
            public BattleCameraComponent battleCamera;

            public BezierPositionComponent bezierPosition;

            public CameraComponent camera;

            public CameraESMComponent cameraEsm;

            public CameraOffsetConfigComponent cameraOffsetConfig;

            public CameraTransformDataComponent cameraTransformData;
        }
    }
}