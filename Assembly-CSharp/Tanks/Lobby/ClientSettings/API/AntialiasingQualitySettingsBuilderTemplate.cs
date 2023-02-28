using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636314016708481587L)]
    public interface AntialiasingQualitySettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template {
        [AutoAdded]
        new AntialiasingQualitySettingsBuilderComponent builder();
    }
}