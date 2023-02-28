using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636325230858790545L)]
    public interface VegetationSettingsBuilderTemplate : GraphicsSettingsBuilderTemplate, ConfigPathCollectionTemplate, Template {
        [AutoAdded]
        new VegetationSettingsBuilderComponent builder();
    }
}