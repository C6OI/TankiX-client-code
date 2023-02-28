using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(636307202297072759L)]
    public interface AnisotropicQualityVariantTemplate : CarouselItemTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        AnisotropicQualityVariantComponent variant();
    }
}