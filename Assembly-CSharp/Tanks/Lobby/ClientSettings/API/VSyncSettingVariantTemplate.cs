using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(1545034359700L)]
    public interface VSyncSettingVariantTemplate : CarouselItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        VSyncSettingVariantComponent variant();
    }
}