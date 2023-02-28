using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636314016810481786L)]
    public interface ShadowQualitySettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template {
        [AutoAdded]
        new ShadowQualitySettingsBuilderComponent builder();
    }
}