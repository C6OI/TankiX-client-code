using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientSettings.API {
    [SerialVersionUID(636053862011098749L)]
    public interface SaturationLevelVariantTemplate : CarouselItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        SaturationLevelVariantComponent variant();
    }
}