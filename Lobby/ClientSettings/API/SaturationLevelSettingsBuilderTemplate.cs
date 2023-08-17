using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientSettings.API {
    [SerialVersionUID(636053878532460683L)]
    public interface SaturationLevelSettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, Template,
        ConfigPathCollectionTemplate {
        [AutoAdded]
        new SaturationLevelSettingsBuilderComponent builder();
    }
}