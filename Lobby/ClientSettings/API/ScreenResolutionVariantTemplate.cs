using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientSettings.API {
    [SerialVersionUID(636045272650536988L)]
    public interface ScreenResolutionVariantTemplate : CarouselItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ScreenResolutionVariantComponent variant();
    }
}