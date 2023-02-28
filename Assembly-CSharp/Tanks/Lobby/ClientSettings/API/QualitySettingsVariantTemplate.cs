using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636044374237217210L)]
    public interface QualitySettingsVariantTemplate : CarouselItemTemplate, Template {
        [AutoAdded]
        QualitySettingsVariantComponent variant();
    }
}