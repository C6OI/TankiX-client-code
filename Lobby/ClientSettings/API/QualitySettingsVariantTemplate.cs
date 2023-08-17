using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientSettings.API {
    [SerialVersionUID(636044374237217210L)]
    public interface QualitySettingsVariantTemplate : CarouselItemTemplate, Template {
        [AutoAdded]
        QualitySettingsVariantComponent variant();
    }
}