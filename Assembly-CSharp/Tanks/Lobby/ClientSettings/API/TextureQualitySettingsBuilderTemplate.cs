using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636314016899690706L)]
    public interface TextureQualitySettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template {
        [AutoAdded]
        new TextureQualitySettingsBuilderComponent builder();
    }
}