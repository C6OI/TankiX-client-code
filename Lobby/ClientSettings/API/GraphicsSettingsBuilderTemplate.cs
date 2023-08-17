using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientSettings.API {
    [SerialVersionUID(636046881855127390L)]
    public interface GraphicsSettingsBuilderTemplate : Template, ConfigPathCollectionTemplate {
        [AutoAdded]
        GraphicsSettingsBuilderComponent builder();
    }
}