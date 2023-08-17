using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapInitSystem : ECSSystem {
        [OnEventFire]
        public void SetMaterialsQuality(NodeAddedEvent e, SingleNode<MapInstanceComponent> map,
            [JoinAll] SingleNode<MaterialsSettingsComponent> materialsSettings) =>
            Shader.globalMaximumLOD = materialsSettings.component.GlobalShadersMaximumLOD;

        [OnEventFire]
        public void SetGrassQuality(NodeAddedEvent e, SingleNode<MapInstanceComponent> map,
            SingleNode<CameraComponent> cameraNode, [JoinAll] SingleNode<VegetationSettingsComponent> grassSettings) {
            VegetationSettingsComponent component = grassSettings.component;

            if (component.GrassFarDrawDistance > 0.1f) {
                ShadowCastingMode mode = grassSettings.component.GrassCastsShadow ? ShadowCastingMode.On
                                             : ShadowCastingMode.Off;

                GrassGenerator[] componentsInChildren = map.component.SceneRoot.GetComponentsInChildren<GrassGenerator>();

                foreach (GrassGenerator grassGenerator in componentsInChildren) {
                    grassGenerator.SetCulling(component.GrassFarDrawDistance,
                        component.GrassNearDrawDistance,
                        component.GrassFadeRange,
                        component.GrassDenstyMultipler);

                    grassGenerator.Generate();
                    SetShadowCastingMode(grassGenerator.transform, mode);
                }
            }
        }

        [OnEventFire]
        public void SetWaterQuality(NodeAddedEvent e, SingleNode<WaterComponent> water,
            [JoinAll] SingleNode<WaterSettingsComponent> waterSettings) {
            if (!waterSettings.component.HasReflection) {
                water.component.DisableReflection();
            }
        }

        void SetShadowCastingMode(Transform root, ShadowCastingMode mode) {
            MeshRenderer[] componentsInChildren = root.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                componentsInChildren[i].shadowCastingMode = mode;
            }
        }
    }
}