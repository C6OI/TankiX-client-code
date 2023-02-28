using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636322849772752558L)]
    public interface VegetationSettingsTemplate : CarouselItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        VegetationSettingsComponent variant();
    }
}