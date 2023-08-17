using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientControls.API {
    [SerialVersionUID(636044459270014040L)]
    public interface CarouselItemTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        CarouselItemTextComponent carouselItemText();
    }
}