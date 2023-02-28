using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(1540219983082L)]
    public interface CartridgeCaseSettingVariantTemplate : CarouselItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        CartridgeCaseSettingVariantComponent variant();
    }
}