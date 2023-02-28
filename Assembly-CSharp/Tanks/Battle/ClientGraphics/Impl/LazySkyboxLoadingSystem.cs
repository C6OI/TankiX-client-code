using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using UnityEngine.Rendering;
using GraphicsSettings = Tanks.Lobby.ClientSettings.API.GraphicsSettings;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class LazySkyboxLoadingSystem : ECSSystem {
        [OnEventFire]
        public void RequestSkyboxResource(NodeAddedEvent e, SingleNode<MapInstanceComponent> mapInstance, SingleNode<LazySkyboxComponet> lazySkybox) {
            if (GoodSystem() && GraphicsSettings.INSTANCE.CurrentQualityLevel > 1 && HDRCompressedTexturesSupported()) {
                if (mapInstance.Entity.HasComponent<SkyBoxDataComponent>()) {
                    RenderSettings.skybox = (Material)mapInstance.Entity.GetComponent<SkyBoxDataComponent>().Data;
                } else {
                    ScheduleEvent(new AssetRequestEvent().Init<SkyBoxDataComponent>(lazySkybox.component.SkyBoxReference.AssetGuid), mapInstance);
                }
            }
        }

        bool HDRCompressedTexturesSupported() => SystemInfo.SupportsTextureFormat(TextureFormat.BC6H) && SystemInfo.SupportsTextureFormat(TextureFormat.BC7);

        bool GoodSystem() => SystemInfo.graphicsMemorySize > 512 || SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D9;

        [OnEventFire]
        public void SetSkybox(NodeAddedEvent e, SingleNode<SkyBoxDataComponent> skyBox) {
            RenderSettings.skybox = (Material)skyBox.component.Data;
        }

        public class SkyBoxDataComponent : ResourceDataComponent { }
    }
}