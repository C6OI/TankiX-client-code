using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636354659801663633L)]
    public interface RenderResolutionQualitySettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template {
        [AutoAdded]
        new RenderResolutionQualitySettingsBuilderComponent builder();
    }
}